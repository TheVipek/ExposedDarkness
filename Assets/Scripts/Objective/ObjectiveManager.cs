using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveManager : MonoBehaviour
{
    
   private Dictionary<Objective,GameObject> objectivesInGame = new Dictionary<Objective, GameObject>();
   [SerializeField] GameObject objectivesUIBox;
   [SerializeField] GameObject objectivePrefab;
   public static ObjectiveManager Instance{get; private set;}
   [SerializeField] ObjectiveHandler[] objectivesInScene; 
   private void Awake() {
      if(Instance != this && Instance != null) Destroy(this);
      else Instance = this;
   }
   private void Start() {
      for(int i = objectivesInScene.Length-1;i>=0;i--)
      {
         objectivesInScene[i].enabled = true;
      }
      
   }
   public void AddObjective(Objective objective)
   {
      GameObject _objectivePrefab =Instantiate(objectivePrefab,objectivesUIBox.transform,false);
      _objectivePrefab.gameObject.GetComponent<ObjectiveText>().objectiveText.text += objective.Description;
      objectivesInGame.Add(objective,_objectivePrefab);
   }
   public void setObjectiveStatus(int objectiveID,ObjectiveStatus status)
   {
      Objective objective = getObjective(objectiveID); 
      GameObject objectiveUI = objectivesInGame[objective];
      if(objective.status==status || objective == null) return;
      if(status == ObjectiveStatus.DONE)
      {
         ObjectiveText objText = objectiveUI.GetComponent<ObjectiveText>();
         TMP_Text text = objText.objectiveText;
         Color32 textColor = text.color;
         text.color = new Color32(textColor.r,textColor.g,textColor.b,32);
         objText.SetObjectiveCheckBox(true);
      }
      objective.status = status;
   }
   public Objective getObjective(int id)
   {
      
      foreach(Objective objective in objectivesInGame.Keys)
      {
         if(objective.Id == id) return objective;
      }
      return null;
      
   }
}
