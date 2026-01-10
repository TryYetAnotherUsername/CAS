using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public static class ConsoleService
{
    public static WinContConsole ConsoleOutput;
    public static event Action OnNewMessageLogged;
    public static List<string> outputStream = new List<string>();

    public static void Print(string message)
    {
        GD.Print(message);
        outputStream.Add(message);
        OnNewMessageLogged?.Invoke();
    }

    static ConsoleService()
    {
        outputStream.Clear();
    }

    public static void LineBreak()
    {
        GD.Print("");
        outputStream.Add("----------");
        OnNewMessageLogged?.Invoke();
    }

    public static void PrintErr(string message)
    {
        GD.PrintErr(message);
        outputStream.Add(message);
        OnNewMessageLogged?.Invoke();
    }

    public static void PushErr(string message)
    {
        GD.PushError(message);
        outputStream.Add(message);
        OnNewMessageLogged?.Invoke();
    }
}
