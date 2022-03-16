using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //para usar las librerias que permiten trabajar con archivos

namespace BlockNotas
{
    public partial class FrmBlock : Form
    {
        public FrmBlock()
        {
            InitializeComponent();
        }

        public static class Global
        {
            public static string rutaBase;
        }

        private TreeNode CreateTree(DirectoryInfo directoryInfo)
        {
            TreeNode treeNode = new TreeNode(directoryInfo.Name);

            foreach (var item in directoryInfo.GetFiles())
            {
                treeNode.Nodes.Add(new TreeNode(item.Name));
            }

            foreach (var item in directoryInfo.GetDirectories())
            {
                treeNode.Nodes.Add(CreateTree(item));
            }

            return treeNode;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbdialog = new FolderBrowserDialog();

                if (fbdialog.ShowDialog() == DialogResult.OK)
                {
                    String globalPath = fbdialog.SelectedPath;
                    treeView1.Nodes.Clear();

                    DirectoryInfo directoryInfo = new DirectoryInfo(globalPath);
                    Global.rutaBase = directoryInfo.FullName;
                    treeView1.Nodes.Add(CreateTree(directoryInfo));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al abrir");
            }
            

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(saveFileDialogB.FileName))
                {
                    string text = saveFileDialogB.FileName;

                    StreamWriter SaveText = File.CreateText(text);
                    SaveText.Write(rtbContenido.Text);
                    SaveText.Flush(); //para liberar memoria del writer
                    SaveText.Close(); //para cerrar recursos
                    MessageBox.Show("Su archivo fue guardado con exito.");

                }
                else
                {
                    saveFileDialogB.Filter = "Text files (*.txt) | *.txt";
                    if (saveFileDialogB.ShowDialog() == DialogResult.OK)
                    {
                        string text = saveFileDialogB.FileName;

                        StreamWriter SaveText = File.CreateText(text);
                        SaveText.Write(rtbContenido.Text);
                        SaveText.Flush();
                        SaveText.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al guardar");
            }

        } 

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbContenido.Clear();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                saveFileDialogB.Filter = "Text files (*.txt) | *.txt";
                if (saveFileDialogB.ShowDialog() == DialogResult.OK) //me enseña la ventana y me manda si el usuario le dio a ok o cancel
                {

                    if (File.Exists(saveFileDialogB.FileName))
                    {
                        string text = saveFileDialogB.FileName;

                        StreamWriter SaveText = new StreamWriter(text);
                        //StreamWriter SaveText = File.CreateText(text);
                        //StreamWriter SaveText = System.IO.File.AppendText(text);
                        SaveText.Write(rtbContenido.Text);
                        SaveText.Flush(); //para liberar memoria del writer
                        SaveText.Close(); //para cerrar recursos

                    }
                    else //sea que exista o no vamos a escribir la informacion, para que se escriba la infor y quede guardada
                    {
                        string text = saveFileDialogB.FileName;

                        StreamWriter SaveText = File.CreateText(text);
                        SaveText.Write(rtbContenido.Text);
                        SaveText.Flush();
                        SaveText.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al guardar");
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string text = Global.rutaBase + "\\" + treeView1.SelectedNode.Text;

            if (File.Exists(text))
            {
                TextReader read = new StreamReader(text);
                rtbContenido.Text = read.ReadToEnd();
                read.Close();
            }
        }

    }
}
