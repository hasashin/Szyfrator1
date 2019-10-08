using System;
using Gtk;
using Szyfrator1;
using System.Numerics;

public partial class MainWindow : Gtk.Window
{
    private Przekatnokolumnowy szyfr;
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        szyfr = new Przekatnokolumnowy();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnCipherButtonClicked(object sender, EventArgs e)
    {
        szyfr.key = KeyCipherEntry.Text;
        szyfr.Encrypt(InputRawText.Buffer.Text);
    }
    protected void OnDecipherButtonClicked(object sender, EventArgs e)
    {
        szyfr.key = KeyCipherEntry.Text;
        szyfr.Encrypt(InputRawText.Buffer.Text);
    }

    protected void OnKeyCipherEntryChanged(object sender, EventArgs e)
    {

        double pow = Math.Pow(3.0, 127.0);
        ulong wpizdu = 3 * Convert.ToUInt64(pow);
        ulong wynik = wpizdu % 256;
        
        MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Other, ButtonsType.Ok, wynik.ToString());
        dlg.Run();
        dlg.Destroy();
    }
}
