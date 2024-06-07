using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sfs2X.Entities.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using TMPro;

public class PrintingManager : MonoBehaviour
{
    public static PrintingManager instance = null;

    public string No, ItemName, Date, HeatNumber, RecoveryAlloy, ValuesSteel, CastingGrade;
    public List<Matlistsave> _newmatlist = new List<Matlistsave>();

    string path = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        path = Application.dataPath + "/Ticket.pdf";  
       
    }

    public void GenerateFile() {
        if (File.Exists(path))
            File.Delete(path);
        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.NewPage();

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            PrintFormate(document);
        
            document.Close();
            writer.Close();
        }



        //StreamWriter writer = new StreamWriter(path, false);
        //writer.WriteLine(string.Format("Ticket Id : {0}",iSFSObject.GetUtfString("TICKET_ID")));
        //var betting = iSFSObject.GetSFSArray("BET_DETAILS");
        //for (int i = 0; i< betting.Count;i++)
        //    writer.WriteLine(string.Format("Bet Number : {0}     BetAmount : {1}", betting.GetSFSObject(i).GetUtfString("BET_NUM"), betting.GetSFSObject(i).GetDouble("BET_AMOUNT")));
        //writer.Close();

        PrintFiles();
    }

    void PrintFiles()
    {
        Debug.Log(path);
        if (path == null)
            return;

        if (File.Exists(path))
        {
            Debug.Log("file found");
            //var startInfo = new System.Diagnostics.ProcessStartInfo(path);
            //int i = 0;
            //foreach (string verb in startInfo.Verbs)
            //{
            //    // Display the possible verbs.
            //    Debug.Log(string.Format("  {0}. {1}", i.ToString(), verb));
            //    i++;
            //}
        }
        else
        {
            Debug.Log("file not found");
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = path;
        //process.StartInfo.Verb = "print";

        process.Start();
        //process.WaitForExit();

    }

    public void PrintFormate(Document document)
    {
        Paragraph p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);

        p = new Paragraph(string.Format("Alloy Addition Suggest Display Report"));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);

        p = new Paragraph(string.Format("(Made by Bhakti Engineers and TechAvtra Technologies)"));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);


        p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Itemname                     : {0}", ItemName)); //iSFSObject.GetUtfString("TICKET_ID"
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Date                              : {0}", Date));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Heat Number                : {0}", HeatNumber));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Recovery of Alloy         : {0}", RecoveryAlloy));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Values in Steel             : {0}", ValuesSteel));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Casting Grade              : {0}", CastingGrade));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" Carbon value changed : {0}", No));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);


        p = new Paragraph(string.Format(" ", ""));
        p.Alignment = Element.ALIGN_LEFT;
        document.Add(p);

        p = new Paragraph(string.Format("   {0}   " + ": {1}         " + " {2}   " + ": {3}         " + " {4}   " + ": {5}         " + " {6}   " + ": {7}         " + " {8}   " + ": {9}         ", _newmatlist[0]._Material, _newmatlist[0]._value, _newmatlist[1]._Material, _newmatlist[1]._value, _newmatlist[2]._Material, _newmatlist[2]._value, _newmatlist[3]._Material, _newmatlist[3]._value, _newmatlist[4]._Material, _newmatlist[4]._value));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);

        p = new Paragraph(string.Format("   {0}  " + ": {1}         " + " {2}   " + ": {3}         " + " {4}   " + ": {5}         " + " {6}   " + ": {7}         " + " {8}   " + ": {9}         ", _newmatlist[5]._Material, _newmatlist[5]._value, _newmatlist[6]._Material, _newmatlist[6]._value, _newmatlist[7]._Material, _newmatlist[7]._value, _newmatlist[8]._Material, _newmatlist[8]._value, _newmatlist[9]._Material, _newmatlist[9]._value));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);

        p = new Paragraph(string.Format("   {0}   " + ": {1}         " + " {2}   " + ": {3}           ", _newmatlist[10]._Material, _newmatlist[10]._value, _newmatlist[11]._Material, _newmatlist[11]._value));
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);



        //p = new Paragraph(string.Format(" ", ""));
        //p.Alignment = Element.ALIGN_LEFT;
        //document.Add(p);

        //p = new Paragraph(string.Format("Suggestions in Kg."));
        //p.Alignment = Element.ALIGN_CENTER;
        //document.Add(p);


        //p = new Paragraph(string.Format(" ", ""));
        //p.Alignment = Element.ALIGN_LEFT;
        //document.Add(p);

        //p = new Paragraph(string.Format("{0}   " + ": {1}               " + " {2}   " + ": {3}               " + " {4}   " + ": {5}               " + " {6}   " + ": {7}               " , _newmatlist[10]._Material, _newmatlist[10]._value, _newmatlist[11]._Material, _newmatlist[11]._value, _newmatlist[12]._Material, _newmatlist[12]._value, _newmatlist[13]._Material, _newmatlist[13]._value));
        //p.Alignment = Element.ALIGN_CENTER;
        //document.Add(p);

        //p = new Paragraph(string.Format(" {0}  " + ": {1}               " + " {2}   " + ": {3}               " + " {4}   " + ": {5}               " + " {6}   " + ": {7}               " , _newmatlist[14]._Material, _newmatlist[14]._value, _newmatlist[15]._Material, _newmatlist[15]._value, _newmatlist[16]._Material, _newmatlist[16]._value, _newmatlist[17]._Material, _newmatlist[17]._value));
        //p.Alignment = Element.ALIGN_CENTER;
        //document.Add(p);

    }

    public void SetValues(string _No, string _ItemName, string _Date, string _HeatNum, string _RecoveryAlloy, string _ValuesSteel, string _CastingGrade, List<Matlistsave> _Matlist)
    {
        No = _No;
        ItemName = _ItemName;
        Date = _Date;
        HeatNumber = _HeatNum;
        RecoveryAlloy = _RecoveryAlloy;
        ValuesSteel = _ValuesSteel;
        CastingGrade = _CastingGrade;
        _newmatlist = _Matlist;

        GenerateFile();
    }
}
