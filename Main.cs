using Godot;
using System;


public class Main : Panel
{
    // Declare member variables here.
    float inputVoltage = 0.0f;
    float actualVoltage = 0.0f;
    bool timerState = false;
    float time = 0.0f;
    float R = 47000.0f;
    float C = 0.001f;
    float tau = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode("HBoxContainer/SetButton").Connect("pressed", this, nameof(_OnSetButtonPressed));
        GetNode("HBoxContainer/ClearButton").Connect("pressed", this, nameof(_OnClearButtonPressed));
        GetNode("HBoxContainer/StartButton").Connect("pressed", this, nameof(_OnStartButtonPressed));
        GetNode("HBoxContainer/StopButton").Connect("pressed", this, nameof(_OnStopButtonPressed));
        tau = R*C;
    }

    public override void _Process(float delta)
    {
        timer(delta);
        VoltageUpdate();

        var HBoxContainer4 = (HBoxContainer)GetNode("VBoxContainer/HBoxContainer4");
        if(timerState && time >= tau && !HBoxContainer4.Visible){
            var SteadyStateTimeValLabel = (Label)GetNode("VBoxContainer/HBoxContainer4/SteadyStateTimeValLabel");
            SteadyStateTimeValLabel.Text = tau.ToString();
            HBoxContainer4.Show();
        }
    }

    private void VoltageUpdate()
    {
        var VoltageValueLabel = (Label)GetNode("VBoxContainer/HBoxContainer/VoltageValueLabel");
        actualVoltage = CapacitorChargingModel(inputVoltage, time);
        VoltageValueLabel.Text = Math.Round(actualVoltage,2).ToString("0.00");
    }

    private void timer(float delta)
    {
        var TimeValueLabel = (Label)GetNode("VBoxContainer/HBoxContainer/TimeValueLabel");
        var timeInt = (int)time;
        TimeValueLabel.Text = timeInt.ToString();

        if (timerState)
        {
            time += delta;
        }
    }

    public void _OnSetButtonPressed()
    {
        var InputVoltageLineEdit = (LineEdit)GetNode("VBoxContainer/HBoxContainer3/InputVoltageLineEdit");
        inputVoltage = InputVoltageLineEdit.Text.ToFloat();
        System.Console.WriteLine(inputVoltage);
        var SetVoltageLineEdit = (LineEdit)GetNode("VBoxContainer/HBoxContainer2/SetVoltageLineEdit");
        SetVoltageLineEdit.Text = inputVoltage.ToString();
    }

    public void _OnClearButtonPressed()
    {
        var InputVoltageLineEdit = (LineEdit)GetNode("VBoxContainer/HBoxContainer3/InputVoltageLineEdit");
        InputVoltageLineEdit.Text = "0";
        var SetVoltageLineEdit = (LineEdit)GetNode("VBoxContainer/HBoxContainer2/SetVoltageLineEdit");
        SetVoltageLineEdit.Text = "0";
        time = 0.0f;
        var HBoxContainer4 = (HBoxContainer)GetNode("VBoxContainer/HBoxContainer4");
        HBoxContainer4.Visible = false;
    }

    public void _OnStartButtonPressed()
    {
        timerState = true;
    }

     public void _OnStopButtonPressed()
    {
        timerState = false;
    }

    public float CapacitorChargingModel(float Vin, float t)
    {
        return Vin*(1.0f-(float)Math.Exp(-t/(R*C)));
    }
}
