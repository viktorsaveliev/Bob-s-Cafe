using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(VisitorHUD))]
public class Visitor : InteractableObject
{
    public VisitorHUD HUD { get; private set; }
    public Animator Animator { get; private set; }

    public float Patience { get; private set; }
    public float Satiety { get; private set; }
    public bool IsWantsSeparately { get; private set; }

    public int OrderInQueue;

    public string GetName => _name;
    public IState CurrentState => _stateMachine.CurrentState;

    private readonly StateMachine _stateMachine = new();

    public bool IsMemberGroup { get; private set; }
    public int MemberGroupID { get; private set; }

    private string _name;

    private Coroutine _secondTimer;
    private float _wasteOfPatience;
    private Chair _usedChair;



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

    public void Init(int orderInQueue, int groupID = -1, float wasteOfPatience = -1)
    {
        transform.position = new Vector3(17f, 0, 0);

        StringBus stringBus = new();
        string name = stringBus.Names[UnityEngine.Random.Range(0, stringBus.Names.Length)];
        string surname = stringBus.SurNames[UnityEngine.Random.Range(0, stringBus.SurNames.Length)];
        _name = $"{name} {surname}";

        _usedChair = null;
        
        Patience = 100;
        Satiety = UnityEngine.Random.Range(1f, 30f);

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

        UnSelect();
    }

    public void SetChair(Chair chair)
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

            Walk(new Vector3(14f, 0, 0), new VisitorDestroyer());
        }
    }

    public void UpdatePatience()
    {
        Patience -= _wasteOfPatience;
        HUD.UpdateBar(Patience / 100f);
    }

    private int GetHappinessLevel()
    {
        int happinessLevel = 0;

        if (Satiety > 90) happinessLevel++;
        if (Patience > 30) happinessLevel++;

        return happinessLevel;
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

    public void Select()
    {
        ShowOutline();

        IsSelected = true;

        ChangeOutlineColor(OutlineType.Selected);
    }

    public void UnSelect()
    {
        IsSelected = false;

        ChangeOutlineColor(OutlineType.MouseStay);
        HideOutline();
    }

    public override void ShowOutline()
    {
        if (IsSelected) return;
        base.ShowOutline();
    }

    public override void HideOutline()
    {
        if (IsSelected) return;
        base.HideOutline();
    }

    #endregion

    #region StateMachine methods

    public void Eating()
    {
        EatingState eatingState = (EatingState)_stateMachine.GetState<EatingState>();
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
        WaitingState waitingState = (WaitingState) _stateMachine.GetState<WaitingState>();
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