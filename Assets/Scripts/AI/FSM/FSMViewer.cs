using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FSMViewer<State, StateInfo> 
    where State : FSMState<StateInfo>, new()
    where StateInfo : FSMStateInfo
{
    public RectTransform UICanvas;
    public RectTransform BaseText;
    private float recursion = 0;
    private float offsety = 0;
    private List<Transform> Texts = new List<Transform>();
       
    public void Update(FSMachine<State,StateInfo> machine)
    {
        foreach (Transform t in Texts)
            GameObject.Destroy(t.gameObject);
        Texts.Clear();
        recursion = 0;
        offsety = 0;
        ShowState(machine.BaseState, true);
    }

    private void AddText(string s)
    {
        offsety++;
        RectTransform t = GameObject.Instantiate<RectTransform>(BaseText);
        t.position = BaseText.position - Vector3.up * offsety * BaseText.rect.height;
        t.transform.SetParent(UICanvas);
        t.GetComponent<Text>().text = s;
        Texts.Add(t);
    }

    public void ShowState(FSMState<StateInfo> state, bool active)
    {
        recursion++;
        string recursionEntete = "";
        for (int i = 0; i < recursion; i++)
            recursionEntete += "- ";
        AddText(recursionEntete + state.GetType().ToString() + (active ? "*":""));
        foreach (FSMState<StateInfo> s in state.SubStates)
            ShowState(s,s == state.ActiveSubState);
        recursion--;
    }
}
