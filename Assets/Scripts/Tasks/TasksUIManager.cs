using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XRTwin.DataManager;

namespace XRTwin.Tasks
{
    public class TasksUIManager : MonoBehaviour
    {
        [SerializeField] TasksDataManager tasksDataManager;
        [SerializeField] TasksVariables tasksVariable;
        [SerializeField] Transform tasksParent;
        [SerializeField] MRTKTMPInputField inputField;
        [SerializeField] Button button;
        [SerializeField] GameObject addTaskCanvas;

        void Start()
        {
            tasksDataManager.onTasksLoaded.AddListener(SpawnTasks);
            button.onClick.AddListener(AddTask);
        }

        
        void Update()
        {
         
        }

        public void SpawnTasks()
        {   
            foreach(KeyValuePair<string,string> kvp in tasksDataManager.tasks)
            {
                Debug.Log(kvp.Value);
                GameObject taskHolder = Instantiate(tasksVariable.TasksReference, tasksParent);
                taskHolder.gameObject.GetComponentInChildren<TextMeshPro>().text = kvp.Value;

                taskHolder.gameObject.GetComponentInChildren<Interactable>().OnClick.AddListener(delegate { RemoveTask(kvp.Key); });
                
                taskHolder.SetActive(true);
            }
        }

        public void RemoveTask(string id)
        {
            tasksDataManager.RemoveTask(id);
        }

        public void AddTask()
        {
            addTaskCanvas.SetActive(false);
            string text = inputField.text;
            tasksDataManager.CreateTask(text);
        }

    }
}
