using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace XRTwin.DataManager
{
    public class TasksDataManager : MonoBehaviour
    {
        readonly string tasklistUrl = "https://xrtwindata.herokuapp.com/googleTask/tasklist";
        readonly string getTasksUrl = "https://xrtwindata.herokuapp.com/googleTask/tasks";
        readonly string createTaskUrl = "https://xrtwindata.herokuapp.com/googleTask/createTask";
        readonly string deleteTaskUrl = "https://xrtwindata.herokuapp.com/googleTask/deleteTask";
        string accessToken;

        void Start()
        {
            //@TODO Replace with PlayerPrefs
            accessToken = "ya29.a0ARrdaM9rCqj-WrMUsSVkqCQYe-QOTBfhcHrwLjY5kWq5mUWihRTd4voxjNxuiO5yhbIqn4E5-sAg6SAPIxZn_u6oz4zl4ygXjATUkhij2yx9I4ROs24ioaZUamWBKbNAZ4Zf-Vg30xaModlE0Eoh_CRZl4v-8A";

            StartCoroutine(GetTaskLists());
        }

        
        void Update()
        {

        }

        IEnumerator GetTaskLists()
        {
            WWWForm taskListForm = new WWWForm();

            taskListForm.AddField("accessToken", accessToken);

            UnityWebRequest taskListRequest = UnityWebRequest.Post(tasklistUrl, taskListForm);
            taskListRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return taskListRequest.SendWebRequest();

            if (taskListRequest.isNetworkError || taskListRequest.isHttpError)
            {
                Debug.LogError(taskListRequest.error);
                yield break;
            }

            JSONNode taskListInfo = JSON.Parse(taskListRequest.downloadHandler.text);
            
            string taskListID = taskListInfo["data"]["items"][0]["id"];
            StartCoroutine(GetTasks(taskListID));
        }

        IEnumerator GetTasks(string taskListId)
        {
            string idTasksList = taskListId.ToString();
            Debug.Log(idTasksList);

            WWWForm tasksForm = new WWWForm();

            tasksForm.AddField("accessToken", accessToken);
            tasksForm.AddField("tasklistIdentifier", idTasksList);

            UnityWebRequest tasksRequest = UnityWebRequest.Post(getTasksUrl, tasksForm);
            tasksRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return tasksRequest.SendWebRequest();

            if (tasksRequest.isNetworkError || tasksRequest.isHttpError)
            {
                Debug.LogError(tasksRequest.error);
                yield break;
            }

            JSONNode tasksInfo = JSON.Parse(tasksRequest.downloadHandler.text);

            Debug.Log(tasksInfo["data"]);
        }
    }
}
