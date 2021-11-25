using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace XRTwin.DataManager
{
    public class TasksDataManager : MonoBehaviour
    {
        readonly string tasklistUrl = "https://xrtwindata.herokuapp.com/googleTask/tasklist";
        readonly string getTasksUrl = "https://xrtwindata.herokuapp.com/googleTask/tasks";
        readonly string createTaskUrl = "https://xrtwindata.herokuapp.com/googleTask/createTask";
        readonly string deleteTaskUrl = "https://xrtwindata.herokuapp.com/googleTask/deleteTask";
        readonly string userInfoUrl = "https://xrtwindata.herokuapp.com/googleTask/userInfo";

        public Dictionary<string, string> tasks;
        public UnityEvent onTasksLoaded;
        public UnityEvent onNameGot;
        public string userName;

        string accessToken;
        string idToken;
        string taskListID;
        

        void Start()
        {
            //@TODO Replace with PlayerPrefs
            accessToken = PlayerPrefs.GetString("accessToken", "Null"); 
            idToken = PlayerPrefs.GetString("idToken", "Null");

            tasks = new Dictionary<string, string>();

            StartCoroutine(GetTaskLists());
            StartCoroutine(GetUserInfo());
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
            
            taskListID = taskListInfo["data"]["items"][0]["id"];
            
            if(taskListID != null || !String.Equals(taskListID, ""))
                StartCoroutine(GetTasks(taskListID));
        }

        IEnumerator GetTasks(string taskListId)
        {
            tasks.Clear();

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
            Debug.Log(tasksInfo["data"]["items"].Count);
            
            int tasksCount = tasksInfo["data"]["items"].Count;
            
            if (tasksCount > 8)
            {
                tasks.Clear();

                for (int i=0; i<8; i++)
                {
                    string id = tasksInfo["data"]["items"][i]["id"];
                    id = id.ToString();

                    string title = tasksInfo["data"]["items"][i]["title"];
                    title = title.ToString();

                    tasks.Add(id, title);
                }
            }
            else
            {
                tasks.Clear();

                for (int i=0; i<tasksCount; i++)
                {
                    string id = tasksInfo["data"]["items"][i]["id"];
                    id = id.ToString();

                    string title = tasksInfo["data"]["items"][i]["title"];
                    title = title.ToString();

                    tasks.Add(id, title);
                }
            }

            onTasksLoaded.Invoke();
        }

        IEnumerator GetUserInfo()
        {
            WWWForm userInfoForm = new WWWForm();

            userInfoForm.AddField("idToken", idToken);

            UnityWebRequest userInfoRequest = UnityWebRequest.Post(userInfoUrl, userInfoForm);
            userInfoRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return userInfoRequest.SendWebRequest();

            if (userInfoRequest.isNetworkError || userInfoRequest.isHttpError)
            {
                Debug.LogError(userInfoRequest.error);
                yield break;
            }

            JSONNode userInfo = JSON.Parse(userInfoRequest.downloadHandler.text);
           
            string name = userInfo["userData"]["name"];
            string[] nameSplit = name.Split();
            userName = nameSplit[0];

            onNameGot.Invoke();
        }


        public void CreateTask(string title)
        {
            StartCoroutine(TaskCreate(title));
        }

        IEnumerator TaskCreate(string title)
        {
            string idTasksList = taskListID.ToString();

            WWWForm createTask = new WWWForm();

            createTask.AddField("accessToken", accessToken);
            createTask.AddField("tasklistIdentifier", idTasksList);
            createTask.AddField("title", title);

            UnityWebRequest createTasksRequest = UnityWebRequest.Post(createTaskUrl, createTask);
            createTasksRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return createTasksRequest.SendWebRequest();

            if (createTasksRequest.isNetworkError || createTasksRequest.isHttpError)
            {
                Debug.LogError(createTasksRequest.error);
                yield break;
            }

            JSONNode createTasksInfo = JSON.Parse(createTasksRequest.downloadHandler.text);
            Debug.Log("Task Create Status");
            Debug.Log(createTasksInfo["data"]);
            
            // Updates the Task Dictionary
            StartCoroutine(GetTasks(taskListID));
        }

        public void RemoveTask(string id)
        {
            StartCoroutine(TaskRemove(id));
        }

        IEnumerator TaskRemove(string id)
        {
            string idTasksList = taskListID.ToString();

            WWWForm removeTask = new WWWForm();

            removeTask.AddField("accessToken", accessToken);
            removeTask.AddField("tasklistIdentifier", idTasksList);
            removeTask.AddField("taskId", id);

            UnityWebRequest removeTasksRequest = UnityWebRequest.Post(deleteTaskUrl, removeTask);
            removeTasksRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return removeTasksRequest.SendWebRequest();

            if (removeTasksRequest.isNetworkError || removeTasksRequest.isHttpError)
            {
                Debug.LogError(removeTasksRequest.error);
                yield break;
            }

            JSONNode createTasksInfo = JSON.Parse(removeTasksRequest.downloadHandler.text);
            Debug.Log("Task Create Status");
            Debug.Log(createTasksInfo["data"]);

            // Updates the Task Dictionary
            StartCoroutine(GetTasks(taskListID));
        }
    }
}
