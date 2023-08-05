using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

using TMPro;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;
//using DG.Tweening;

public class ClientManager : MonoBehaviour
{
    public GameObject clientListPrefab;
    public Transform clientListParent;
    public TMP_Dropdown filterDropdown;
    public TMP_Text dataDescription;
    public TMP_Text popupDataDisplay;
    public TMP_Text waitText;
    public GameObject popupWindow;

    private List<Client> clientsData;
    private List<GameObject> clientListItems;

    private Dictionary<int, Client> clientsDatasss;


    private IEnumerator Start()
    {
        string url = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
           
            waitText.text = "Wait Until  Fetching the Data";
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                //Debug.LogError("Failed to fetch client data: " + webRequest.error);
                waitText.text = "Failed to fetch client data: " + webRequest.error;
                yield break;
            }
            waitText.text = "";
            string jsonData = webRequest.downloadHandler.text.Trim();

            LoadClientsData(jsonData);
            CreateClientList();
            filterDropdown.onValueChanged.AddListener(OnFilterDropdownValueChanged);
        }
    }

    //private void Start()
    //{
    //    LoadClientsData();
    //    CreateClientList();
    //    filterDropdown.onValueChanged.AddListener(OnFilterDropdownValueChanged);
    //}

    private void LoadClientsData(string data)
    {

        //clientsData.Clear();

        // TextAsset jsonText = Resources.Load<TextAsset>("clientsData");
        clientsData =  JsonConvert.DeserializeObject<ClientDataWrapper>(data).clients;

        RootObjectNew clientsDatass  = JsonConvert.DeserializeObject<RootObjectNew>(data);      
        clientsDatasss = new Dictionary<int, Client>();
        foreach (var clients in clientsDatass.data)
        {
            clientsDatasss.Add(int.Parse(clients.Key), new Client
            {
                name = clients.Value.name,
                points = clients.Value.points,
                address = clients.Value.address

            });
            
            
        }
    }

  
    private void CreateClientList()
    {
        clientListItems = new List<GameObject>();

        foreach (Client client in clientsData)
        {
            GameObject listItem = Instantiate(clientListPrefab, clientListParent);
            TMP_Text label = listItem.GetComponentInChildren<TMP_Text>();
            //label.text = $"{client.label} - Points: {client.points}";
            label.text = $"{client.label}" ;
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() => ShowClientPopup(client.id));

            clientListItems.Add(listItem);
        }
    }
    [System.Serializable]
    public class RootObjectNew
    {

        public Dictionary<string, Client> data;
        
    }
    private void OnFilterDropdownValueChanged(int index)
    {
        ClearClientList();

        if (index == 0) // All clients
            CreateClientList();
        else if (index == 1) // Managers only
            CreateFilteredClientList(true);
        else if (index == 2) // Non-managers
            CreateFilteredClientList(false);
    }

    // ...

    private void CreateFilteredClientList(bool isManager)
    {
        List<Client> filteredClients = clientsData.FindAll(client => client.isManager == isManager);

        foreach (Client client in filteredClients)
        {
            GameObject listItem = Instantiate(clientListPrefab, clientListParent);
            TMP_Text label = listItem.GetComponentInChildren<TMP_Text>();
            //label.text = $"{client.label} - Points: {client.points}";
            label.text = $"{client.label}";
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() => ShowClientPopup(client.id));
            
            clientListItems.Add(listItem);
        }
    }

    private void ClearClientList()
    {
        foreach (GameObject listItem in clientListItems)
        {
            Destroy(listItem);
        }
        clientListItems.Clear();
    }

    private void ShowClientPopup(int id)//Client client)
    {
        if (id == 4) id -= 1;
        if (clientsDatasss.TryGetValue(id, out Client client) )
        {
            dataDescription.text = $"Client {id} Data:";
         //   popupDataDisplay.text = "Name: " + client.name;
            popupDataDisplay.text = "Name: " + client.name + "\n" +
                 "Points: " + client.points + "\n" +
                  "Address: " + client.address;
        }
        else
        {
            dataDescription.text = $"Client {id} Data: ";
            popupDataDisplay.text = $"Sorry Client 5 Data is not Available yet!";
 

        }
        popupWindow.SetActive(true);
        popupWindow.transform.DOScale(Vector3.one * 2.5f, 0.3f);
        clientListParent.gameObject.SetActive(false);
    }

    public void ClosePopup()
    {
      popupWindow.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => popupWindow.SetActive(false));
        clientListParent.gameObject.SetActive(true);

    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
           Application.Quit();
#endif
        //
    }
}

[System.Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
    public string address;
    public string name;
    public int points;
}

[System.Serializable]
public class ClientDataWrapper
{
    

    public List<Client> clients;
    public Dictionary<string, Client> data;

}
