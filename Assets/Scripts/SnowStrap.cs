using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class SnowStrap : MonoBehaviour 
{
    public Mesh snowMesh;
    public Material snowMaterial;
    public Text countText;
    public Text fpsText;
    
    public int step = 10; // 1フレームあたりに作る数
    
    // FPS
    private float m_updateInterval = 0.5f;
    private float m_accum;
    private int m_frames;
    private float m_timeleft;
    private float m_fps;
    
    // オブジェクト数
    private int count = 0;

    private EntityManager entityManager;
    private EntityArchetype snowArchetype;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private void Start()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();

        snowArchetype = entityManager.CreateArchetype(
            typeof(TransformMatrix),
            typeof(Position),
            typeof(Rotation),
            typeof(MeshInstanceRenderer)
        );
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            for (int i = 0; i < step; i++)
            {
                var snow = entityManager.CreateEntity(snowArchetype);
        
                entityManager.SetSharedComponentData(snow, new MeshInstanceRenderer
                {
                    mesh = snowMesh,
                    material = snowMaterial,
                });
        
                entityManager.SetComponentData(snow, new Position
                {
                    Value = new float3(Random.Range(-20.0f, 20.0f), 20, Random.Range(-20.0f, 20.0f))
                });
                entityManager.SetComponentData(snow, new Rotation
                {
                    Value = Quaternion.Euler(0f, Random.Range(0.0f, 180.0f), 90f)
                });

                count++;
                
            }
            
            countText.text = count.ToString();
            
            // FPS
            m_timeleft -= Time.deltaTime;
            m_accum += Time.timeScale / Time.deltaTime;
            m_frames++;

            if ( 0 < m_timeleft ) return;

            m_fps = m_accum / m_frames;
            m_timeleft = m_updateInterval;
            m_accum = 0;
            m_frames = 0;

            fpsText.text = "FPS: " + m_fps.ToString("f2");
        }
    }
}
