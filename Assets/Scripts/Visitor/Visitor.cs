using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(VisitorHUD))]
public class Visitor : InteractableObject
{
    private readonly StateMachine _stateMachine = new();
    private readonly VisitorDestroyer _visitorDestroyer = new();

    public IState CurrentState => _stateMachine.CurrentState;

    private Coroutine _secondTimer;
    private float _wasteOfPatience;
    private Chair _usedChair;

    public VisitorHUD HUD { get; private set; }
    public Animator Animator { get; private set; }

    public string Name { get; private set; }
    public int OrderInQueue { get; set; }
    public bool IsMemberGroup { get; private set; }
    public int MemberGroupID { get; private set; }
    public float Patience { get; private set; }
    public float Satiety { get; private set; }
    public bool IsWantsSeparately { get; private set; }

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();

        HUD = GetComponent<VisitorHUD>();
        Animator = GetComponent<Animator>();

        InitStates();
    }

    private void OnEnable()
    {
        _secondTimer = StartCoroutine(SecondTimer());
    }

    private void OnDisable()
    {
        if (_secondTimer != null)
            StopCoroutine(_secondTimer);
    }

    #endregion

    public void Init(int orderInQueue, int groupID = -1, float wasteOfPatience = -1)
    {
        ResetData();
        SetRandomName();
        UnSelect();

        _wasteOfPatience = wasteOfPatience == -1 ? UnityEngine.Random.Range(0.5f, 3f) : wasteOfPatience;
        OrderInQueue = orderInQueue;

        if(groupID != -1)
        {
            IsMemberGroup = true;
            MemberGroupID = groupID;
        }
        else
        {
            IsMemberGroup = false;
            MemberGroupID = -1;
        }
    }

    public void SetChairForSitting(Chair chair)
    {
        if (_usedChair != null) return;
        _usedChair = chair;
    }

    public void UpdateSatiety()
    {
        Satiety += UnityEngine.Random.Range(2f, 4f);
        if(Satiety >= 100)
        {
            if (IsSelected)
            {
                UnSelect();
                EventBus.OnPlayerUnSelectVisitor?.Invoke(this);
            }

            Walk(new Vector3(14f, 0, 0), _visitorDestroyer);
        }
    }

    public void UpdatePatience()
    {
        Patience -= _wasteOfPatience;
        HUD.UpdateBar(Patience / 100f);
    }

    public void PayForService()
    {
        GameDataConfig difficultyLevel = GameData.Settings.CurrentDifficultyLevel;
        if (GetHappinessLevel() > 1)
        {
            Money.Give(difficultyLevel.MoneyForHapinessVisitor);
        }
        else
        {
            Money.Give(difficultyLevel.MoneyForDissatisfiedVisitor);
        }
    }

    private void ResetData()
    {
        transform.position = new Vector3(17f, 0, 0);
        _usedChair = null;
        Patience = 100;
        Satiety = UnityEngine.Random.Range(1f, 30f);
        IsSelected = false;
    }

    private void SetRandomName()
    {
        StringBus stringBus = new();

        string firstName = stringBus.Names[UnityEngine.Random.Range(0, stringBus.Names.Length)];
        string lastName = stringBus.SurNames[UnityEngine.Random.Range(0, stringBus.SurNames.Length)];

        Name = $"{firstName} {lastName}";
    }

    private int GetHappinessLevel()
    {
        int happinessLevel = 0;

        if (Satiety > 90) happinessLevel++;
        if (Patience > 30) happinessLevel++;

        return happinessLevel;
    }

    #region Interactable methods

    protected override void HandleClick()
    {
        if (!IsSelected)
        {
            Select();
            EventBus.OnPlayerSelectVisitor?.Invoke(this);
        }
        else
        {
            UnSelect();
            EventBus.OnPlayerUnSelectVisitor?.Invoke(this);
        }
    }

    protected override void OnSelected()
    {
        //EventBus.OnPlayerSelectVisitor?.Invoke(this);
    }

    protected override void OnUnSelected()
    {
        //EventBus.OnPlayerUnSelectVisitor?.Invoke(this);
    }

    #endregion

    #region StateMachine methods

    public void Eating()
    {
        EatingState eatingState = (EatingState) _stateMachine.GetState<EatingState>();
        eatingState.UpdateData(_usedChair);
        _stateMachine.ChangeState(eatingState);
    }

    public void Walk(Vector3 targetPosition, IVisitorActionAfterPointReached action = null)
    {
        WalkingState walkingState = (WalkingState) _stateMachine.GetState<WalkingState>();
        walkingState.UpdateData(targetPosition, action);
        _stateMachine.ChangeState(walkingState);
    }

    public void Waiting()
    {
        IState waitingState = _stateMachine.GetState<WaitingState>();
        _stateMachine.ChangeState(waitingState);
    }

    private IEnumerator SecondTimer()
    {
        WaitForSeconds waitForSeconds = new(1f);
        yield return waitForSeconds;

        while (_stateMachine.CurrentState != null)
        {
            _stateMachine.CurrentState.Update();
            yield return waitForSeconds;
        }
    }

    private void InitStates()
    {
        _stateMachine.StateMap = new Dictionary<Type, IState>
        {
            [typeof(WalkingState)] = new WalkingState(this, Animator),
            [typeof(WaitingState)] = new WaitingState(this, Animator),
            [typeof(EatingState)] = new EatingState(this, Animator)
        };
    }

    #endregion
}