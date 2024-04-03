using Sirenix.OdinInspector;
using UnityEngine;

public enum FoodType
{
    DontMatter,
    Meat,
    Leaf
}

public enum DinoFoodState
{
    Free,
    InEatProcess,
    InAviaryFeeder
}

[RequireComponent(typeof(InteractableObject))]
public class DinoFoodItem : TakableItem
{
    [SerializeField] private FoodType _foodType;
    [SerializeField, Range(0, 100)] private float _foodValue;
    [SerializeField, ReadOnly] private DinoFoodState _foodState;

    public DinoFoodState FoodState => _foodState;
    public FoodType FoodType => _foodType;
    public float FoodValue => _foodValue;

    private void Start()
    {
        _foodState = DinoFoodState.Free;
    }

    public void ChangeFoodState(DinoFoodState state) => _foodState = state;
}
