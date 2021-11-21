using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XRTwin.DataManager;

namespace XRTwin.Tasks
{
    public class TasksUIManager : MonoBehaviour
    {
        [SerializeField] TasksDataManager tasksDataManager;
        [SerializeField] TasksVariables tasksVariable;
        [SerializeField] Transform tasksParent;

        void Start()
        {
            tasksDataManager.onTasksLoaded.AddListener(SpawnTasks);
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

    }
}
