using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class intractable : MonoBehaviour
{
   public bool useEvents;
   [SerializeField]public string PromptMassage;
 
   public virtual string OnLook(){
      return PromptMassage;
   }
   public void BaseIntract(){
      if(useEvents)
          GetComponent<IntractionEvent>().OnIntract.Invoke();
      Intract();
   }
   protected virtual void Intract()
   {

   }
}
