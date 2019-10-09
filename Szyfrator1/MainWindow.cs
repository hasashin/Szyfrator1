using System;
using Gtk;
using Szyfrator1;
using System.Collections;

public partial class MainWindow : Window
{
    private readonly ArrayList keyCharsTable = new ArrayList
    { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
      'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
      'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    private readonly ArrayList messageCharsTable = new ArrayList
    {
        'a','b','c','d','e','f','g','h','i','j','k',
        'l','m','n','o','p','q','r','s','t','u','v',
        'w','x','y','z','A','B','C','D','E','F','G',
        'H','I','J','K','L','M','N','O','P','Q','R',
        'S','T','U','V','W','X','Y','Z','.',' ','_'};
    private Przekatnokolumnowy szyfr;
    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();
        MenuBar mainMenu = new MenuBar();
        Menu plik = new Menu();
        plik.AddMnemonicLabel(new Label("Plik"));
        mainMenu.Add(plik);
        Add(mainMenu);
        plik.Show();
        mainMenu.Show();
        szyfr = new Przekatnokolumnowy();
    }

    private bool ShowTrimWarning()
    {
        MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo, "W tekście znaleziono nieprawidłowe znaki. Wiadomość zostanie zaszyfrowana bez nich.\nCzy chcesz kontynuować?");
        ResponseType result = (ResponseType)dlg.Run();
        dlg.Destroy();
        return result == ResponseType.Yes;
    }

    private bool TrimWrongChars(TextView textBox)
    {
        try
        {
            foreach (char letter in textBox.Buffer.Text)
            {
                if (!messageCharsTable.Contains(letter))
                {
                    textBox.Buffer.Text = textBox.Buffer.Text.Replace(letter.ToString(), "");
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    private bool ValidateText(TextView textBox)
    {
        bool warningResponse = true;
        foreach (char letter in textBox.Buffer.Text)
        {
            if (!messageCharsTable.Contains(letter))
            {
                warningResponse = ShowTrimWarning();
                break;
            }
        }
        if (warningResponse)
        {
            return TrimWrongChars(textBox);
        }
        return false;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnCipherButtonClicked(object sender, EventArgs e)
    {
        szyfr.key = KeyCipherEntry.Text;
        bool response= false;
        try
        {
            response = ValidateText(InputRawText);
        }
        catch (Exception ex)
        {
            MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, ex.Message);
            dlg.Title = "Wystąpił błąd";
            dlg.Run();
            dlg.Destroy();
        }
        if(response) CipherOutput.Buffer.Text = szyfr.Encrypt(InputRawText.Buffer.Text);
    }
    protected void OnDecipherButtonClicked(object sender, EventArgs e)
    {
        szyfr.key = KeyDecipherEntry.Text;
        RawOutput.Buffer.Text = szyfr.Decrypt(InputSecretText.Buffer.Text);
    }

    protected void OnKeyCipherEntryChanged(object sender, EventArgs e)
    {
        Entry keyEntry = (sender as Entry);
        keyEntry.Text = keyEntry.Text.ToUpper();
        foreach(char letter in keyEntry.Text)
        {
            if (!keyCharsTable.Contains(letter))
            {
                keyEntry.Text = keyEntry.Text.Replace(letter.ToString(), "");
            }
        }
    }
}
