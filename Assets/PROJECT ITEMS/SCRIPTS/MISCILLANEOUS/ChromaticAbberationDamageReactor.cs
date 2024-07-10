using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChromaticAbberationDamageReactor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private DamageEventSO DamageEventSO;
    ChromaticAberration _postProcessingScript;
    private void Awake()
    {
        _postProcessingScript = GetComponent<ChromaticAberration>();
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TriggerPostProcessing()
    {
        if (_postProcessingScript != null)
        {
            _postProcessingScript.radius = new Vector2(3,3);
            _postProcessingScript.intensity = 5;
            _postProcessingScript.hardness = 5;
            CPeffect = clearPostProcessing();
        }
    }
    Task CPeffect;
    async Task clearPostProcessing()
    {
        
        float intensity = _postProcessingScript.intensity;
        float hardness = _postProcessingScript.hardness;
        await Task.Delay(2000);
        while(intensity > .5)
        {
            intensity -= .01f;
            hardness -= .01f;
            _postProcessingScript.intensity = intensity;
            _postProcessingScript.hardness = hardness;
            await Task.Delay(2);
        }
    }
    private void OnEnable()
    {
        DamageEventSO.damageEvent.AddListener(TriggerPostProcessing);
        DamageEventSO.RegisterListener(gameObject);
    }
    private void OnDisable()
    {
        DamageEventSO.damageEvent.RemoveListener(TriggerPostProcessing);
        

    }
}
