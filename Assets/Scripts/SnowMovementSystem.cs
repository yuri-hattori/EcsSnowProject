using Unity.Entities;
using Unity.Transforms;

public class SnowMovementSystem : ComponentSystem {
    
    // 落下速度
    private float delta = 0.1f;

    public struct SnowGroup
    {
        public ComponentDataArray<Position> snowPostion;
        public int Length;
    }

    [Inject] private SnowGroup _snowGroup;
    
    protected override void OnUpdate()
    {
        for (int i = 0; i < _snowGroup.Length; i++)
        {
            var newPos = _snowGroup.snowPostion[i];
            newPos.Value.y -= delta;
            _snowGroup.snowPostion[i] = newPos;
        }
    }
}
