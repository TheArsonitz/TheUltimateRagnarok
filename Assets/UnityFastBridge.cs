using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways] // Permette al server di girare anche senza premere Play
public class UnityFastBridge : MonoBehaviour
{
    private HttpListener listener;
    private Queue<string> commandQueue = new Queue<string>();

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void AutoStart()
    {
        EditorApplication.delayCall += () =>
        {
            var existing = UnityEngine.Object.FindObjectOfType<UnityFastBridge>();
            if (existing == null)
            {
                GameObject go = new GameObject("ServerMCP_Persistent");
                go.hideFlags = HideFlags.HideAndDontSave;
                go.AddComponent<UnityFastBridge>();
            }
        };
    }
#endif

    void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= UpdateQueue;
        EditorApplication.update += UpdateQueue;
#endif
        try
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(OnRequest), listener);
            Debug.Log("[God Mode Bridge] Attivo e in ascolto...");
        }
        catch (Exception e)
        {
            Debug.LogError("Errore avvio Bridge: " + e.Message);
        }
    }

    void OnRequest(IAsyncResult result)
    {
        if (listener == null || !listener.IsListening) return;
        var context = listener.EndGetContext(result);
        var request = context.Request;

        using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
        {
            string json = reader.ReadToEnd();
            lock (commandQueue) { commandQueue.Enqueue(json); }
        }

        var response = context.Response;
        byte[] buffer = Encoding.UTF8.GetBytes("{\"status\":\"eseguito\"}");
        response.ContentLength64 = buffer.Length;
        response.ContentType = "application/json";
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();

        listener.BeginGetContext(new AsyncCallback(OnRequest), listener);
    }

    void Update()
    {
        UpdateQueue();
    }

    void UpdateQueue()
    {
        lock (commandQueue)
        {
            while (commandQueue.Count > 0)
            {
                ExecuteCommand(commandQueue.Dequeue());
            }
        }
    }

    void ExecuteCommand(string json)
    {
        Command cmd = JsonUtility.FromJson<Command>(json);
        if (cmd == null) return;

        if (cmd.action == "log")
        {
            Debug.Log("[Antigravity]: " + cmd.message);
        }
        else if (cmd.action == "refresh")
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("[God Mode Bridge] AssetDatabase.Refresh() eseguito!");
#endif
        }
        else if (cmd.action == "run_task")
        {
            // Esegue qualsiasi classe creata da Antigravity tramite Reflection
            Type type = Type.GetType(cmd.message + ", Assembly-CSharp");

            if (type != null)
            {
                MethodInfo method = type.GetMethod("Execute", BindingFlags.Static | BindingFlags.Public);
                if (method != null)
                {
                    method.Invoke(null, null);
                    Debug.Log($"[God Mode] Task '{cmd.message}' eseguito con successo!");
                }
                else
                {
                    Debug.LogError($"[God Mode] Metodo 'public static void Execute()' non trovato in {cmd.message}");
                }
            }
            else
            {
                Debug.LogError($"[God Mode] Classe {cmd.message} non trovata. Se l'hai appena creata, aspetta 2 secondi che Unity compili e riprova.");
            }
        }
    }

    void OnDisable() // Rilascia la porta 8080 quando Unity ricompila il codice
    {
#if UNITY_EDITOR
        EditorApplication.update -= UpdateQueue;
#endif
        if (listener != null)
        {
            listener.Stop();
            listener.Close();
            listener = null;
        }
    }

    [Serializable]
    public class Command { public string action; public string message; }
}