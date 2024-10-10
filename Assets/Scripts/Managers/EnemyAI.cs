using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static UnityEngine.UI.CanvasScaler;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<AIGameStateSO> gameStates;

    private Player player;

    private float gameStateTimer;
    private float gameStateTimerMax = 1f;
    private AlertMetrics alertMetrics;
    private ActionType actionType;
    private CardSO actionCard;

    private CardSO worker; // 1
    private CardSO defense; // 1
    private CardSO economy; // 0 or 1
    private List<CardSO> meleeUnits = new List<CardSO>(); // 2+
    private List<CardSO> rangedUnits = new List<CardSO>(); // 1+
    private List<CardSO> spells = new List<CardSO>(); // 1+

    private void Awake()
    {
        actionType = ActionType.None;
    }

    private void Start()
    {
        player = PlayerRed.Instance;
        ApplyDifficulty();
        ReadLoadout();
    }

    private void Update()
    {
        gameStateTimer += Time.deltaTime;
        if (gameStateTimer >= gameStateTimerMax)
        {
            DetermineGameState();
            ExecuteAction();
            gameStateTimer = 0f;
        }   
    }

    private void ApplyDifficulty()
    {
        float multiplier = 0;
        switch (GameManager.Instance.GetDifficulty())
        {
            case 0: //Easy - 100% stats
                multiplier = 1;
                break;
            case 1: //Medium - 150% stats
                multiplier = 1.5f;
                break;
            case 2: //Hard - 200% stats
                multiplier = 2;
                break;
        }

        foreach (CardSO card in player.GetLoadout())
        {
            switch (card.cardType)
            {
                case CardSO.CardType.Building:
                    BuildingCardSO buildingCard = card as BuildingCardSO;
                    buildingCard.Health = IncreaseStatInList(buildingCard.Health, multiplier);

                    if (buildingCard.BuildingType == BuildingType.Defense)
                        buildingCard.Attack = IncreaseStatInList(buildingCard.Attack, multiplier);

                    break;
                case CardSO.CardType.Character:
                    CharacterCardSO characterCard = card as CharacterCardSO;
                    characterCard.Health = IncreaseStatInList(characterCard.Health, multiplier);
                    characterCard.Attack = IncreaseStatInList(characterCard.Attack, multiplier);

                    break;
                case CardSO.CardType.Spell:
                    SpellCardSO spellCard = card as SpellCardSO;
                    spellCard.Attack = IncreaseStatInList(spellCard.Attack, multiplier);

                    break;
                case CardSO.CardType.Worker:
                    CharacterCardSO workerCard = card as CharacterCardSO;
                    workerCard.Health = IncreaseStatInList(workerCard.Health, multiplier);

                    break;
            }
        }
    }
    private List<int> IncreaseStatInList(List<int> stats, float multiplier)
    {
        List<int> newStats = new List<int>();
        foreach (int stat in stats)
        {
            int newStat = (int)(stat * multiplier);
            newStats.Add(newStat);
        }

        return newStats;
    }

    private void ReadLoadout()
    {
        foreach (CardSO card in player.GetLoadout())
        {
            switch (card.cardType)
            {
                case CardSO.CardType.Building:
                    BuildingCardSO buildingCard = card as BuildingCardSO;
                    if (buildingCard.BuildingType == BuildingType.Defense)
                        defense = card;
                    else
                        economy = card;
                    break;
                case CardSO.CardType.Character:
                    CharacterCardSO characterCard = card as CharacterCardSO;
                    switch (characterCard.CharacterType)
                    {
                        case CharacterType.Melee:
                            meleeUnits.Add(card);
                            break;
                        case CharacterType.Ranged:
                            rangedUnits.Add(card);
                            break;
                        case CharacterType.Worker:
                            worker = card;
                            break;
                    }
                    break;
                case CardSO.CardType.Spell:
                    spells.Add(card);
                    break;
                case CardSO.CardType.Worker:
                    worker = card;
                    break;
            }
        }
    }

    #region Game State Calculations
    private void DetermineGameState()
    {
        AlertMetrics newAlertMetrics;
        CalculateAOC(out newAlertMetrics.areaOfControl);
        CalculateEMS(out newAlertMetrics.effectiveMilitaryStrength);
        CalculateGPM(out newAlertMetrics.goldPerMinute);

        if (!alertMetrics.Equals(newAlertMetrics))
        {
            alertMetrics = newAlertMetrics;
            ChooseAction();
        }
    }

    private void CalculateAOC(out AlertLevel areaOfControl)
    {
        float mapSizeMultiplier = GameManager.Instance.GetMapSize() / 30;
        float AOC = player.GetFurthestControlledArea() / PlayerBlue.Instance.GetFurthestControlledArea();

        if (AOC > mapSizeMultiplier)
            areaOfControl = AlertLevel.Favored;
        else if (AOC < (1 / mapSizeMultiplier))
            areaOfControl = AlertLevel.Unfavored;
        else
            areaOfControl = AlertLevel.Even;
    }

    private void CalculateEMS(out AlertLevel effectiveMilitaryStrength)
    {
        float EMS = GetEMSFromList(player.GetSpawnedMilitary()) / (float)GetEMSFromList(PlayerBlue.Instance.GetSpawnedMilitary());

        if (EMS > 2)
            effectiveMilitaryStrength = AlertLevel.Favored;
        else if (EMS < .5f)
            effectiveMilitaryStrength = AlertLevel.Unfavored;
        else
            effectiveMilitaryStrength = AlertLevel.Even;
    }

    private int GetEMSFromList(List<GameObject> MilitaryUnits)
    {
        int EMS = 0;
        foreach (GameObject unit in MilitaryUnits)
            EMS += unit.GetComponent<Character>().GetUnitStrength();

        return EMS;
    }

    private void CalculateGPM(out AlertLevel goldPerMinute)
    {
        float GPM = GetGPMFromList(player.GetSpawnedEconomy()) / (float)GetGPMFromList(PlayerBlue.Instance.GetSpawnedEconomy());

        if (GPM > 1.5f)
            goldPerMinute = AlertLevel.Favored;
        else if (GPM < .666f)
            goldPerMinute = AlertLevel.Unfavored;
        else
            goldPerMinute = AlertLevel.Even;
    }

    private int GetGPMFromList(List<GameObject> EconomyUnits)
    {
        int GPM = 600;
        foreach (GameObject unit in EconomyUnits)
        {
            if (unit.GetComponent<Character>() != null)
                GPM += unit.GetComponent<Character>().GetAttack() * 3;
            else if (unit.GetComponent<Building>() != null)
                GPM += unit.GetComponent<Building>().GetAttack() * 60;
        }

        return GPM;
    }
    #endregion

    #region Deciding Actions
    private void ChooseAction()
    {
        List<int> actionOrder = new List<int>();

        foreach (AIGameStateSO gameStateSO in gameStates)
        {
            if (alertMetrics.areaOfControl == gameStateSO.AOC && alertMetrics.effectiveMilitaryStrength == gameStateSO.EMS && alertMetrics.goldPerMinute == gameStateSO.GPM)
            {
                actionOrder = GetActionOrder(new List<int> (gameStateSO.DecisionTable));
                break;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            bool actionFound = false;
            switch (actionOrder[i])
            {
                case 1: //Develop or Upgrade Economy
                    actionFound = DevelopOrUpgradeEconomy();
                    break;
                case 2: //Upgrade Military
                    actionFound = UpgradeMilitary();
                    break;
                case 3: //Build Defense
                    actionFound = BuildDefense();
                    break;
                case 4: //Develop Military or Cast Spell
                    actionFound = DevelopMilitaryOrCastSpell();
                    break;
            }

            if (actionFound)
                break;
        }
    }

    private List<int> GetActionOrder(List<int> decisionTable)
    {
        int totalWeight = 100;
        List<int> weights = new List<int> { 70, 24, 5, 1 };
        List<int> actionOrder = new List<int>();
        
        for (int i = 4; i > 0; i--)
        {
            int rand = Random.Range(0, totalWeight);
 
            for (int j = 0; j < i; j++)
            {
                if (rand < weights[j])
                {
                    actionOrder.Add(decisionTable[j]);
                    totalWeight -= weights[j];
                    decisionTable.Remove(decisionTable[j]);
                    weights.Remove(weights[j]);            
                    break;
                }

                rand -= weights[j];
            }
        }

        return actionOrder;
    }

    private bool DevelopOrUpgradeEconomy()
    {
        int numWorkers = player.GetNumberOfWorkers();
        if (numWorkers < GameManager.Instance.GetMaxWorkerAmount() / 2)
        {
            actionType = ActionType.Spawn;
            actionCard = worker;
            Debug.Log($"AI wants to spawn worker");
            return true;
        }
        else if (numWorkers < GameManager.Instance.GetMaxWorkerAmount())
        {
            switch (alertMetrics.goldPerMinute)
            {
                case AlertLevel.Unfavored:
                    return EconomyBranch1();
                case AlertLevel.Even:
                    int chanceBlue = 50;

                    if (worker.level != worker.upgradeCost.Count)
                        return EconomyBranch2(chanceBlue);
                    else if (economy != null)
                        if (economy.level != economy.upgradeCost.Count)
                            return EconomyBranch2(chanceBlue);

                    return EconomyBranch1();
                case AlertLevel.Favored:
                    chanceBlue = 25;

                    if (worker.level != worker.upgradeCost.Count)
                        return EconomyBranch2(chanceBlue);
                    else if (economy != null)
                        if (economy.level != economy.upgradeCost.Count)
                            return EconomyBranch2(chanceBlue);

                    return EconomyBranch1();
            }
        }
        else 
        {
            if (worker.level != worker.upgradeCost.Count)
            {
                actionType = ActionType.Upgrade;
                actionCard = worker;
                Debug.Log($"AI wants to upgrade worker");
                return true;
            }
            else if (economy != null)
            {
                if (!MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
                {
                    actionType = ActionType.Build;
                    actionCard = economy;
                    Debug.Log($"AI wants to build economy");
                    return true;
                }
                else if (MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().GetBuilding().GetCard().BuildingType == BuildingType.Economy
                    && economy.level != economy.upgradeCost.Count)
                {
                    actionType = ActionType.Upgrade;
                    actionCard = economy;
                    Debug.Log($"AI wants to upgrade economy");
                    return true;
                }
            }
        }

        return false;
    }
    private bool EconomyBranch1() //Build Economy
    {
        if (economy == null)
        {
            actionType = ActionType.Spawn;
            actionCard = worker;
            Debug.Log($"AI wants to spawn worker");
        }
        else
        {
            int x = (GameManager.Instance.GetMaxWorkerAmount() - player.GetNumberOfWorkers()) * 10;
            if (MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
                x += 0;
            else
                x += 50;

            int rand = Random.Range(0, x);

            if (x > 50 && rand <= 50)
            {
                actionType = ActionType.Build;
                actionCard = economy;
                Debug.Log($"AI wants to build economy");
            }
            else
            {
                actionType = ActionType.Spawn;
                actionCard = worker;
                Debug.Log($"AI wants to spawn worker");
            } 
        }

        return true;
    }
    private bool EconomyBranch2(int chanceBlue) //Upgrade Economy
    {
        int rand = Random.Range(0, 100);

        if (rand < chanceBlue)
            return EconomyBranch1();

        actionType = ActionType.Upgrade;

        if (economy == null)
            actionCard = worker;
        else if (economy.level == economy.upgradeCost.Count)
            actionCard = worker;
        else if (worker.level == worker.upgradeCost.Count)
            actionCard = economy;
        else
        {
            int x = 0;
            Building building = MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().GetBuilding();
            if (building != null)
            {
                if (building.GetCard().BuildingType == BuildingType.Economy)
                    x = 50;
                else
                    x = -100;
            }
            else
                x = -75;

            rand = Random.Range(-100, 100) + x;

            if (rand < 0)
            {
                actionCard = worker;
                Debug.Log($"AI wants to upgrade worker");
            }
            else
            {
                actionCard = economy;
                Debug.Log($"AI wants to upgrade economy");
            }
        }
        return true;
    }

    private bool UpgradeMilitary()
    {
        if (MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
        {
            if (MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().GetBuilding().GetCard().BuildingType == BuildingType.Defense)
            {
                if (alertMetrics.areaOfControl == AlertLevel.Unfavored || 
                    (alertMetrics.areaOfControl == AlertLevel.Even && alertMetrics.effectiveMilitaryStrength == AlertLevel.Unfavored))
                {
                    actionType = ActionType.Upgrade;
                    actionCard = defense;
                    Debug.Log($"AI wants to upgrade defense");
                    return true;
                }
            }
        }

        Dictionary<string, CardSO> cards = new Dictionary<string, CardSO>();
        Dictionary<string, int> weights = new Dictionary<string, int>();
        int xMax = 0;

        foreach (CardSO card in meleeUnits)
        {
            if (card.level == card.upgradeCost.Count)
                continue;

            if (card.timesCasted > 0)
            {
                cards.Add(card.name, card);
                weights.Add(card.name, card.timesCasted * 10);
                xMax += card.timesCasted * 10;
            }
        }

        foreach (CardSO card in rangedUnits)
        {
            if (card.level == card.upgradeCost.Count)
                continue;

            if (card.timesCasted > 0)
            {
                cards.Add(card.name, card);
                weights.Add(card.name, card.timesCasted * 10);
                xMax += card.timesCasted * 10;
            }
        }

        foreach (CardSO card in spells)
        {
            if (card.level == card.upgradeCost.Count)
                continue;

            if (card.timesCasted > 0)
            {
                cards.Add(card.name, card);
                weights.Add(card.name, card.timesCasted * 10);
                xMax += card.timesCasted * 10;
            }
        }

        foreach (GameObject unit in player.GetSpawnedMilitary())
        {
            if (weights.ContainsKey(unit.name))
            {
                weights[unit.name] += 10;
                xMax += 10;
            }
        }

        if (xMax == 0)
            return false;

        int rand = Random.Range(0, xMax);
        foreach (string key in weights.Keys)
        {
            if (rand < weights[key])
            {
                actionType = ActionType.Upgrade;
                actionCard = cards[key];
                Debug.Log($"AI wants to upgrade {actionCard}");
                return true;
            }
            else
                rand -= weights[key];
        }

        return true;
    }
    private bool BuildDefense()
    {
        if (player.playerColor == Player.PlayerColor.Blue)
        {
            if (!MapManager.Instance.buildingSlots[0].GetComponent<BuildingSlot>().ContainsBuilding())
            {
                actionType = ActionType.Build;
                actionCard = defense;
                Debug.Log($"AI wants to build defense");
                return true;
            }
        }
        else if (player.playerColor == Player.PlayerColor.Red)
        {
            if (!MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
            {
                actionType = ActionType.Build;
                actionCard = defense;
                Debug.Log($"AI wants to build defense");
                return true;
            }
        }

        return false;
    }
    private bool DevelopMilitaryOrCastSpell()
    {
        int numEnemies = GetNumEnemiesNearFurthestUnit();
        int spellChance = (5 * (int)Mathf.Pow(numEnemies, 2)) - (5 * numEnemies);
        int rand = Random.Range(0, 100);

        if (rand < spellChance)
        {
            actionType = ActionType.Cast;
            actionCard = spells[Random.Range(0, spells.Count)];
            Debug.Log($"AI wants to cast {actionCard}");
        }
        else
        {
            actionType = ActionType.Spawn;
            actionCard = ChooseMilitaryUnitToSpawn();
            Debug.Log($"AI wants to spawn {actionCard}");
        }

        return true;
    }
    private int GetNumEnemiesNearFurthestUnit()
    {
        int numEnemies = 0;
        float detectionRange = 4f;
        foreach (GameObject unit in player.GetSpawnedMilitary())
        {
            int numColliders = 0;
            Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, detectionRange, unit.GetComponent<Entity>().GetTargetLayer());
            foreach (Collider collider in hitColliders)
                if (collider is BoxCollider)
                    numColliders++;

            if (numColliders > numEnemies)
                numEnemies = numColliders;
        }

        return numEnemies;
    }
    private CharacterCardSO ChooseMilitaryUnitToSpawn()
    {
        Dictionary<CharacterCardSO, float> cardList = new Dictionary<CharacterCardSO, float>();
        CharacterCardSO unitToSpawn = null;
        int rangedCount = 0, meleeCount = 0;
        float cumulativeWeight = 0;

        //Calculate unit weights using spawned units
        foreach (GameObject unit in player.GetSpawnedMilitary())
        {
            if (unit.GetComponent<Character>() != null)
            {
                CharacterCardSO card = unit.GetComponent<Character>().GetCard();
                if (card.CharacterType == CharacterType.Ranged)
                    rangedCount++;
                else if (card.CharacterType == CharacterType.Melee)
                    meleeCount++;
            }
        }

        //Generate list of weights for each spawnable unit
        foreach (CharacterCardSO card in meleeUnits)
        {
            float unitValue = 0;
            unitValue = card.GetCardStrength() / (float)card.cardCost[card.level - 1] * (1 + ((rangedCount - meleeCount) * 0.1f));

            if (unitValue > 0)
            {
                cardList.Add(card, unitValue);
                cumulativeWeight += unitValue;
            }
        }

        foreach (CharacterCardSO card in rangedUnits)
        {
            float unitValue = 0;
            unitValue = card.GetCardStrength() / (float)card.cardCost[card.level - 1] * (1 + ((meleeCount - rangedCount) * 0.1f));
            
            if (unitValue > 0)
            {
                cardList.Add(card, unitValue);
                cumulativeWeight += unitValue;
            }
        }

        //Get random unit based on random float generated
        float rand = Random.Range(0, cumulativeWeight);
        foreach (CharacterCardSO card in cardList.Keys)
        {
            if (rand <= cardList[card])
            {
                unitToSpawn = card;
                break;
            }

            rand -= cardList[card];
        }

        return unitToSpawn;
    }
    #endregion  
    
    private void ExecuteAction()
    {
        int Gold = player.GetGold();
        switch (actionType)
        {
            case ActionType.Spawn:
                if (Gold >= actionCard.cardCost[actionCard.level - 1])
                {
                    if (actionCard.cardType == CardSO.CardType.Character)
                    {
                        Vector3 spawnPos = Vector3.zero;
                        if (CheckEnemyInBase())
                            spawnPos = player.GetBaseLocation();
                        else
                        {
                            float attackRange = (actionCard as CharacterCardSO).AttackRange;
                            if (player.playerColor == Player.PlayerColor.Blue)
                                spawnPos = new Vector3(player.GetBaseLocation().x + player.GetFurthestControlledArea() - attackRange, 0.5f, 0);
                            else
                                spawnPos = new Vector3(player.GetBaseLocation().x - player.GetFurthestControlledArea() + attackRange, 0.5f, 0);
                        }

                        player.SpawnCharacter(actionCard, spawnPos);
                    }
                    else
                    {
                        if (player.playerColor == Player.PlayerColor.Blue)
                            player.SpawnWorker(actionCard, MapManager.Instance.mines[0]);
                        else
                            player.SpawnWorker(actionCard, MapManager.Instance.mines[1]);
                    }
                    Debug.Log("AI is spawning a " + actionCard.name);
                    ChooseAction();
                }
                break;
            case ActionType.Upgrade:
                if (Gold >= actionCard.upgradeCost[actionCard.level - 1])
                {
                    player.SubtractGold(actionCard.upgradeCost[actionCard.level - 1]);
                    actionCard.IncreaseCardLevel();
                    Debug.Log("AI upgraded " + actionCard.name + " from level " + (actionCard.level - 1) + " to level " + actionCard.level);
                    ChooseAction();
                }
                break;
            case ActionType.Build:
                if (Gold >= actionCard.cardCost[actionCard.level - 1])
                {
                    if (CheckEnemyInBase())
                        ChooseAction();
                    else
                    {
                        if (player.playerColor == Player.PlayerColor.Blue)
                        {
                            player.BuildBuilding(actionCard, MapManager.Instance.buildingSlots[0]);
                            Debug.Log("AI is building a " + actionCard.name);
                            ChooseAction();
                        }
                        else if (player.playerColor == Player.PlayerColor.Red)
                        {
                            player.BuildBuilding(actionCard, MapManager.Instance.buildingSlots[2]);
                            Debug.Log("AI is building a " + actionCard.name);
                            ChooseAction();
                        }
                    }
                }
                break;
            case ActionType.Cast:
                if (Gold >= actionCard.cardCost[actionCard.level - 1])
                {
                    Vector3 castPos = Vector3.zero;
                    if (player.playerColor == Player.PlayerColor.Blue)
                    {
                        castPos = PlayerRed.Instance.GetFurthestUnitPos();
                        castPos = new Vector3(castPos.x + (float)(actionCard as SpellCardSO).Size / 3, castPos.y, castPos.z);
                    }
                    else
                    {
                        castPos = PlayerBlue.Instance.GetFurthestUnitPos();
                        castPos = new Vector3(castPos.x - (float)(actionCard as SpellCardSO).Size / 3, castPos.y, castPos.z);
                    }

                    player.SpawnSpell(actionCard, castPos);
                    Debug.Log("AI is casting " + actionCard.name);
                    ChooseAction();
                }
                break;
            case ActionType.None:
                break;
        }
    }
    
    public bool CheckEnemyInBase()
    {
        if (player.playerColor == Player.PlayerColor.Blue)
        {
            float unitDistance = Vector3.Distance(PlayerBlue.Instance.GetBaseLocation(), PlayerRed.Instance.GetFurthestUnitPos());
            float buildingDistance = Vector3.Distance(PlayerBlue.Instance.GetBaseLocation(), MapManager.Instance.buildingSlots[0].transform.position);
            if (unitDistance < buildingDistance)
                return true;
            
        }
        else if (player.playerColor == Player.PlayerColor.Red)
        {
            float unitDistance = Vector3.Distance(PlayerRed.Instance.GetBaseLocation(), PlayerBlue.Instance.GetFurthestUnitPos());
            float buildingDistance = Vector3.Distance(PlayerRed.Instance.GetBaseLocation(), MapManager.Instance.buildingSlots[2].transform.position);
            if (unitDistance < buildingDistance)
                return true;
        }

        return false;
    }
}

public enum AlertLevel
{
    Unfavored,
    Even,
    Favored
}

[System.Serializable]
public struct AlertMetrics
{
    public AlertLevel areaOfControl;
    public AlertLevel effectiveMilitaryStrength;
    public AlertLevel goldPerMinute;
}

public enum ActionType
{
    None,
    Spawn,
    Upgrade,
    Build,
    Cast
}