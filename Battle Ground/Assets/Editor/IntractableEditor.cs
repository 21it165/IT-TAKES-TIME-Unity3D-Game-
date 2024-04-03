using UnityEditor;

[CustomEditor(typeof(intractable),true)]
public class IntractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
      intractable intract = (intractable)target;
      if(target.GetType() == typeof(EventOnlyIntractable)){
            
            intract.PromptMassage = EditorGUILayout.TextField(" Prompt Massage", intract.PromptMassage);
            EditorGUILayout.HelpBox("EventOnyIntract can ONLY use UnityEvents.", MessageType.Info);
            if(intract.GetComponent<IntractionEvent>() == null){
                    intract.useEvents = true;
                    intract.gameObject.AddComponent<IntractionEvent>();
            }

      }
      else{
        base.OnInspectorGUI();
        if(intract.useEvents){
             if(intract.GetComponent<IntractionEvent>() == null)
                intract.gameObject.AddComponent<IntractionEvent>();
        }
        else{
               if(intract.GetComponent<IntractionEvent>() != null)
                      DestroyImmediate(intract.GetComponent<IntractionEvent>());
        }                        
      }
   }
}
