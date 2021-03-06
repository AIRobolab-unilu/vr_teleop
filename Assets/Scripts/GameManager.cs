﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public HeaderController statusHeaderController;
    public ContentController contentController;

    public DialogController dialogController;

    public GesturesController gesturesController;
    public HeaderController gesturesHeaderController;


    private string motivationStatus = "";
    private string hadrwareStatus = "";
    private string motorsStatus = "";
    private string dialogStatus = "";

    private string previousDialogStatus = "";

    private bool updateStatus = false;
    private bool updateHardware = false;
    private bool updateMotivation = false;
    private bool updateDialog = false;
    private bool updateMotors = false;

    

    public bool UpdateHeadMovement { get; private set; }
    public bool UpdateHandsMovement { get; private set; }
    public bool InitPointLeft { get; set; }
    public bool InitPointRight { get; set; }
    public bool GestureNavigation { get; private set; }
    public string Gesture { get; set; }

    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame() {
        this.Gesture = null;
    }

    

    //Update is called every frame.
    void Update() {

        //this.DecodeStatus("\n\n\n\n");

        //this.motivationStatus = "Curiosity 2$Frustration 4";

        if (this.updateMotivation && !this.motivationStatus.Equals("")) {

            foreach (var items in this.motivationStatus.Split(':')) {
                string[] tokens = items.Split('/');

                
                //name, low limit, min, value, hight limit, max
                this.contentController.UpdateValue(tokens[0], tokens[3]);
            }
        }

        if (this.updateMotors && !this.motorsStatus.Equals("")) {

            Debug.Log(motorsStatus);
            //string[] tokens = this.motorsStatus.Split(':');

            foreach (var items in this.motorsStatus.Split(':')) {
                //Debug.Log(items);
                string[] tokens = items.Split('/');

                this.contentController.UpdateValue(tokens[0], tokens[1]);
            }


            /*this.contentController.UpdateValue("Neck horizontal", tokens[0]);
            this.contentController.UpdateValue("Neck vertical", tokens[1]);
            this.contentController.UpdateValue("Left arm horizontal", tokens[2]);
            this.contentController.UpdateValue("Left arm vertical", tokens[3]);
            this.contentController.UpdateValue("Left elbow", tokens[4]);
            this.contentController.UpdateValue("Right arm horizontal", tokens[5]);
            this.contentController.UpdateValue("Right arm vertical", tokens[6]);
            this.contentController.UpdateValue("Right elbow", tokens[7]);*/
            
        }

        if (this.updateHardware && !this.hadrwareStatus.Equals("")) {

            
            string[] tokens = this.hadrwareStatus.Split(':');

            //CPU, RAM
            this.contentController.UpdateValue("CPU", tokens[0]);
            this.contentController.UpdateValue("RAM", tokens[1]);

        }


        this.dialogStatus = "set a timer please:passive:how much time:partial_create_callback$(('set', 'create', 'make'), ('timer',)):how much time:0/0";
        if (this.updateDialog && !this.dialogStatus.Equals("") && this.previousDialogStatus != this.dialogStatus) {

            this.previousDialogStatus = this.dialogStatus;

            string[] tokens = this.dialogStatus.Split(':');

            Debug.Log(dialogStatus);

            string question = tokens[0];
            string mode = tokens[1];
            string answer = tokens[2];
            string alternatives = tokens[3];
            string optionals = tokens[4];
            string chosen = tokens[5];

            //alternatives = "callback$((ok,one))";
            //alternatives += "/test$re";

            this.dialogController.SetStatus(question, mode, answer, alternatives, optionals, chosen);

            

        }
    }

    public void UpdateGestures(string gestures) {

        this.gesturesController.Reset();
        
        foreach(string gesture in gestures.Split(':')) {
            this.gesturesController.Add(gesture);
        }

    }

    public string GetTitleFromButton(string buttonName) {
        if (buttonName.StartsWith("Status")) {
            if (buttonName.EndsWith("Left")) {
                return "Hardware Status";
            }
            else if (buttonName.EndsWith("Top")) {
                return "Dialog State";
            }
            else if (buttonName.EndsWith("Right")) {
                return "Motivational Component Data";
            }
            else if (buttonName.EndsWith("Bottom")) {
                return "Motor Values";
            }

        }
        else if (buttonName.StartsWith("Commands")) {
            if (buttonName.EndsWith("Left")) {
                return "Toggle Arms Control";
            }
            else if (buttonName.EndsWith("Top")) {
                return "Record";
            }
            else if (buttonName.EndsWith("Right")) {
                return "Toggle Head Control";
            }
            else if (buttonName.EndsWith("Bottom")) {
                return "Play";
            }
        }
        return "";
    }

    public void DecodeStatus(string status) {
        //Debug.Log(status);
        string[] tokens = status.Split('\n');

        this.motivationStatus = tokens[0];
        this.hadrwareStatus = tokens[1];
        this.motorsStatus = tokens[2];
        this.dialogStatus = tokens[3];
        //this.dialogStatus = "what's your name:static:I am QT Robot::my name is QT Robot/I am QT Robot:I am QT Robot";

        
    }

    private void ResetStatusUI() {
        
        updateStatus = false;
        updateHardware = false;
        updateMotivation = false;
        updateDialog = false;
        updateMotors = false;

        this.ResetDialog();
        this.ResetStatus();
        
    }

    public void PlayGesture() {
        string selected = this.gesturesController.GetSelected();
        Debug.Log("Playing gesture : " + selected);
        this.Gesture = selected;

        this.ResetGestures();
        this.GestureNavigation = false;
    }

    private void ResetStatus() {

        this.contentController.Reset();
        this.statusHeaderController.HideAll();
        
    }

    private void ResetDialog() {
        this.dialogController.Reset();
        this.dialogController.HideAll();

    }

    private void ResetGestures() {
        //this.gesturesController.Reset();
        this.gesturesHeaderController.HideAll();
        
    }

    private void ShowStatusWithTitle(string title) {

        this.ResetStatusUI();

        this.statusHeaderController.ShowAll();
        this.statusHeaderController.SetHeader(title);

        this.contentController.Reset();
    }

    private void ShowGesturesWithTitle(string title) {

        this.ResetGestures();

        this.gesturesHeaderController.ShowAll();
        this.gesturesHeaderController.SetHeader(title);

        //this.gesturesController.Reset();
    }

    private void ShowDialog(string title) {

        this.ResetStatusUI();

        this.dialogController.ShowAll(title);
    }

    //Show hardware status
    public void StatusLeft() {
        Debug.Log("<color=green>Status Left button pressed</color>");

        this.ShowStatusWithTitle(this.GetTitleFromButton("StatusLeft"));



        this.updateHardware = true;
    }

    //Show emotions status
    public void StatusRight() {
        Debug.Log("<color=green>Status Right button pressed</color>");

        this.ShowStatusWithTitle(this.GetTitleFromButton("StatusRight"));

        //this.contentController.Add("Curiosity", "0");
        //this.contentController.Add("Frustration", "0");
        //this.contentController.Add("Pain", "0");
        //this.contentController.Add("Frustration", "0");

        this.updateMotivation = true;


    }

    public void CommandsLeft() {
        Debug.Log("<color=green>Commands Left button pressed</color>");

        this.ResetGestures();

        this.UpdateHandsMovement = !this.UpdateHandsMovement;
        this.InitPointLeft = true;
        this.InitPointRight = true;
    }

    //Control the head
    public void CommandsRight() {
        Debug.Log("<color=green>Commands Right button pressed</color>");

        this.ResetGestures();

        this.UpdateHeadMovement = !this.UpdateHeadMovement;


    }

    //Show dialog status
    public void StatusTop() {
        Debug.Log("<color=green>Status Up button pressed</color>");

        this.ShowDialog(this.GetTitleFromButton("StatusTop"));

        this.updateDialog = true;
    }

    

    //Show motors status
    public void StatusBottom() {
        Debug.Log("<color=green>Status Down button pressed</color>");

        this.ShowStatusWithTitle(this.GetTitleFromButton("StatusBottom"));

        this.updateMotors = true;
    }

    public void CommandsTop() {
        Debug.Log("<color=green>Commands Up button pressed</color>");

        this.ResetGestures();

    }

    public void CommandsBottom() {

        this.GestureNavigation = !this.GestureNavigation;

        if (this.GestureNavigation) {
            this.ShowGesturesWithTitle(this.GetTitleFromButton("CommandsBottom"));
        }
        else {
            this.ResetGestures();
        }

        /*this.gesturesController.Add("first");
        this.gesturesController.Add("second");
        this.gesturesController.Add("third");
        this.gesturesController.Add("forth");
        this.gesturesController.Add("fifth");


        this.UpdateGestures("one:two:three");*/



        Debug.Log("<color=green>Commands Down button pressed</color>");
    }
}