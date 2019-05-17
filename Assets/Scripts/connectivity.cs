using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;


public class connectivity : MonoBehaviour {
    private string connectionstring;


    // Use this for initialization
    void Start () {

        Debug.Log("Connecting to database...");
         connectionstring =  @"Data Source = 127.0.0.1; 
     user id = Ness;
     password = memo;
     Initial Catalog = VideojuegoBD;";



        SqlConnection dbConnection = new SqlConnection(connectionstring);
        

        try
        {
            
            dbConnection.Open();
            Debug.Log("Connected to database.");
        }
        catch (SqlException _exception)
        {
            Debug.LogWarning(_exception.ToString());
           
        }


        //  conn.Close();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

