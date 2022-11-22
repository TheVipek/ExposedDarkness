using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveList : MonoBehaviour
{
    
   [SerializeField] List<Objective> objectivesInGame;
   [SerializeField] GameObject objectivesUIBox;
   [SerializeField] GameObject objectivePrefab;
   public static ObjectiveList instance;
   public delegate void OnStatusChange();
   public static event OnStatusChange onStatusChange;
   private void Awake() {
      if(instance != this && instance != null)
      {
         Destroy(this);
      }else
      {
         instance = this;
      }
   }
   private void Start() {
      InitObjectives();
   }
   public void InitObjectives()
   {
      foreach (var item in objectivesInGame)
         {
            GameObject _objectivePrefab =Instantiate(objectivePrefab,objectivesUIBox.transform,false);
            _objectivePrefab.gameObject.GetComponent<ObjectiveText>().objectiveText.text += item.description;
            item.objectiveUI = _objectivePrefab;
         }
   }
   public void setObjectiveStatus(Objective objective,ObjectiveStatus status)
   {
      if(objective.status==status) return;
      if(status == ObjectiveStatus.DONE)
      {
         ObjectiveText objText = objective.objectiveUI.GetComponent<ObjectiveText>();
         TMP_Text text = objText.objectiveText;
         Color32 textColor = text.color;
         text.color = new Color32(textColor.r,textColor.g,textColor.b,32);
         objText.SetObjectiveCheckBox(true);
         //text.fontStyle = FontStyles.Underline;
         // text.fontStyle = FontStyles.Strikethrough | FontStyles.SmallCaps;
         //text.fontStyle = FontStyles.SmallCaps;
      }
      objective.status = status;
   }
   public Objective getObjective(int id)
   {
      foreach (var item in objectivesInGame)
         {
            if(item.id == id)
            {
               return item;
            }

         }
      return null;
      
   }
}
