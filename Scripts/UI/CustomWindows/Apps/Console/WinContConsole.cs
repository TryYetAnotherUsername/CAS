using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WinContConsole : Control
{
	[Export] private LineEdit Command;
	[Export] private LineEdit Args;
	[Export] private Button Exe;
	[Export] private RichTextLabel Output;
    public static readonly string WelcomeMessage = 
        "[color=#636366][code]----------[/code][/color][color=white]CAS Console Welcome[/color][color=#636366][code]----------[/code][/color]\n" +
        "[color=white]->\tType commands in the bottom fields and press execute to run it.\n" +
        "\t\tCommands are automatically converted to [/color][code]snake_case[/code][color=white], as per the convention in CAS.\n" +
        "->\tFor a list of available commands to play around with, run:\n" +
        "\tCommand: [/color][color=yellow][code]help[/code][/color]\n" +
        "\tArguments: [color=#8E8E93][code]<leave blank>[/code][/color]\n" +
        "[color=#636366]this message can be re-printed by opening a new console window[/color]\n" +
        "[color=#636366][code]--------------------------------------[/code][/color]";


    public override void _EnterTree()
    {
        Exe.Pressed += ExeCommand;
        ConsoleService.OnNewMessageLogged += Update;
        ConsoleService.Print(WelcomeMessage);
    }

    public override void _ExitTree()
    {
        Exe.Pressed -= ExeCommand;
        ConsoleService.OnNewMessageLogged -= Update;
    }



	public void Update()
    {
        if (ConsoleService.outputStream == null)
        {
            return;
        }

        Output.Text = "";

		foreach (string item in ConsoleService.outputStream)
        {
            Output.AppendText("[code]" + item + "[/code] \n ");
        }
    }

	private void ExeCommand()
    {
		string cmd = Command.Text;
		string arg = Args.Text;
        Relay.Exe(cmd, arg);
    }

}
