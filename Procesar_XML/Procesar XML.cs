using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using objExcel = Microsoft.Office.Interop.Excel;

namespace Procesar_XML
{
    public partial class btnCargar : Form
    {
        public btnCargar()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Cuadro de dialogo y funciones de la ventana del programa */
            OpenFileDialog Archivos = new OpenFileDialog();
            Archivos.Title = "Seleccione los archivos XML para procesar";
            Archivos.Multiselect = true;
            Archivos.Filter = "Archivos XML|*.xml";
            int i = 1;

            /* Ruta escritorio */
            /*string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);*/

            /* Ruta Raiz */
            string ruta = System.Windows.Forms.Application.StartupPath;

            /* Funciones para crear el archivo excel */
            objExcel.Application objAplicacion = new objExcel.Application();
            Workbook objLibro = objAplicacion.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet objHoja = (Worksheet)objAplicacion.ActiveSheet;
            objAplicacion.Visible = false;


            try
            {
                if (Archivos.ShowDialog() == DialogResult.OK)
                {
                    if (Archivos.FileName != null)
                    {
                        /* Contador para barra de progreso */
                        int ContadorProgreso = 1;

                        /* For para ejecutar cada XML y juntar el resultado */
                        foreach (string itemfile in Archivos.FileNames)
                        {

                            /* Muestra el nombre del archivo en el programa*/
                            NombreArchivoLabel.Text = "Nombre del archivo:\n\n" + itemfile;

                            /* Int para calidar la cantidad de XML */
                            int ProgresoTotal = Archivos.FileNames.Length;

                            /* Funcion para acceder a los XML */
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(itemfile);

                            /* Namespace para xPath (Necesario para que entienda las direcciones) */ 
                            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                            nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                            nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                            nsmgr.AddNamespace("xades141", "http://uri.etsi.org/01903/v1.4.1#");
                            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                            nsmgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");
                            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                            nsmgr.AddNamespace("sts", "dian:gov:co:facturaelectronica:Structures-2-1");

                            
                            /* Valores extraidos del XML */
                            XmlNode NitEmisor = xmlDoc.SelectSingleNode("/*/cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID", nsmgr);
                            XmlNode RazonSocial = xmlDoc.SelectSingleNode("/*/cac:AccountingSupplierParty/cac:Party/cac:PartyName", nsmgr);
                            XmlNode EmisionFactura = xmlDoc.SelectSingleNode("/*/cbc:IssueDate", nsmgr);
                            XmlNode ID = xmlDoc.SelectSingleNode("/*/cbc:ID", nsmgr);
                            XmlNode Divisa = xmlDoc.SelectSingleNode("/*/cbc:DocumentCurrencyCode", nsmgr);

                            /* Aplicar formato a la fecha de la emisión */
                            EmisionFactura.InnerText = string.Join("", EmisionFactura.InnerText.Split('-'));

                            /* Factura - Codigo - Razón social */
                            string FACCODRAZ = "FAC " + ID.InnerText + " " + RazonSocial.InnerText;
                            int TOTAL = 0;

                            /* Valores productos y codigos (NOTA: Se evaluan casos exentos dentro de este FOR siempre y cuando estén en línea en el XML)*/
                            XmlNodeList Valores = xmlDoc.SelectNodes("/*/cac:TaxTotal/cac:TaxSubtotal", nsmgr);                            
                            foreach (XmlNode Valor in Valores)
                            {
                                objHoja.Cells[i, 1] = "FCD";
                                objHoja.Cells[i, 3] = ContadorProgreso;
                                objHoja.Cells[i, 5] = EmisionFactura.InnerText;
                                objHoja.Cells[i, 6] = EmisionFactura.InnerText;
                                objHoja.Cells[i, 10] = "N";
                                objHoja.Cells[i, 11] = NitEmisor.InnerText;
                                objHoja.Cells[i, 12] = RazonSocial.InnerText;
                                objHoja.Cells[i, 17] = FACCODRAZ;
                                objHoja.Cells[i, 19] = "D";
                                objHoja.Cells[i, 20] = Divisa.InnerText;

                                string Codigo = "";
                                string ValorPrecio = "";
                                foreach (XmlNode Precio in Valor.SelectSingleNode("cbc:TaxableAmount", nsmgr))
                                {
                                    ValorPrecio = Precio.InnerText;
                                    float flt1 = float.Parse(ValorPrecio);
                                    int entero = (int)Math.Round(flt1);
                                    TOTAL = TOTAL + entero;
                                    objHoja.Cells[i, 18] = entero;
                                    objHoja.Cells[i, 21] = entero;
                                }

                                foreach (XmlNode IVA in Valor.SelectSingleNode("cac:TaxCategory/cbc:Percent", nsmgr))
                                {
                                    float ValorIVA = float.Parse(IVA.InnerText);
                                    if (ValorIVA == 19)
                                    {
                                        Codigo = "62050103";
                                    }
                                    else if (ValorIVA == 5)
                                    {
                                        Codigo = "62050101";
                                    }
                                    else if (ValorIVA == 0)
                                    {
                                        Codigo = "62050501";
                                    }
                                    objHoja.Cells[i, 8] = Codigo;
                                    Codigo = "";
                                    i ++;
                                }
                            }

                            /* Valores IVA y codigos */
                            XmlNodeList Valores2 = xmlDoc.SelectNodes("/*/cac:TaxTotal/cac:TaxSubtotal", nsmgr);
                            foreach (XmlNode Valor in Valores2)
                            {
                                string Codigo = "";
                                int ValorPrecio;
                                foreach (XmlNode Precio in Valor.SelectSingleNode("cbc:TaxAmount", nsmgr))
                                {
                                    
                                    float RomperBase0 = float.Parse(Precio.InnerText);
                                    ValorPrecio = (int)Math.Round(RomperBase0);
                                    if (RomperBase0 != 0)
                                    {
                                        objHoja.Cells[i, 18] = ValorPrecio;
                                        objHoja.Cells[i, 21] = ValorPrecio;
                                        TOTAL = TOTAL + ValorPrecio;
                                    }
                                    else
                                    {
                                        Codigo = "0";
                                    }
                                }
                                if (Codigo != "0")
                                {
                                    foreach (XmlNode IVA in Valor.SelectSingleNode("cac:TaxCategory/cbc:Percent", nsmgr))
                                    {
                                        float ValorIVA = float.Parse(IVA.InnerText);
                                        if (ValorIVA == 19)
                                        {
                                            Codigo = "24080210";
                                        }
                                        else if (ValorIVA == 5)
                                        {
                                            Codigo = "24080201";
                                        }
                                        objHoja.Cells[i, 8] = Codigo;
                                        Codigo = "";
                                    }
                                    objHoja.Cells[i, 1] = "FCD";
                                    objHoja.Cells[i, 3] = ContadorProgreso;
                                    objHoja.Cells[i, 5] = EmisionFactura.InnerText;
                                    objHoja.Cells[i, 6] = EmisionFactura.InnerText;
                                    objHoja.Cells[i, 10] = "N";
                                    objHoja.Cells[i, 11] = NitEmisor.InnerText;
                                    objHoja.Cells[i, 12] = RazonSocial.InnerText;
                                    objHoja.Cells[i, 17] = FACCODRAZ;
                                    objHoja.Cells[i, 19] = "D";
                                    objHoja.Cells[i, 20] = Divisa.InnerText;
                                    i ++;
                                }
                            }

                            /* Acceder al nodo del total de la factura */
                            XmlNode TotalFactura = xmlDoc.SelectSingleNode("/*/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", nsmgr);
                            float ValorTotalFactura = float.Parse(TotalFactura.InnerText);

                            /* Validar exento - En los casos que no estén alineados en el primer FOR */
                            /********************************************************************************/
                            /* $EvaluarDiferencia$ = Total hasta ahora, más uno, para validar la diferencia */
                            int EvaluarDiferencia = TOTAL + 1;
                            if (EvaluarDiferencia < ValorTotalFactura)
                            {
                                /* Calcular el total exento y convertirlo en entero */
                                float TotalExento = ValorTotalFactura - TOTAL;
                                int ValorExento = (int)Math.Round(TotalExento);            
                                TOTAL = TOTAL + ValorExento;

                                objHoja.Cells[i, 18] = ValorExento;
                                objHoja.Cells[i, 21] = ValorExento;

                                string Codigo = "62050501";
                                objHoja.Cells[i, 8] = Codigo;
                                objHoja.Cells[i, 1] = "FCD";
                                objHoja.Cells[i, 3] = ContadorProgreso;
                                objHoja.Cells[i, 5] = EmisionFactura.InnerText;
                                objHoja.Cells[i, 6] = EmisionFactura.InnerText;
                                objHoja.Cells[i, 10] = "N";
                                objHoja.Cells[i, 11] = NitEmisor.InnerText;
                                objHoja.Cells[i, 12] = RazonSocial.InnerText;
                                objHoja.Cells[i, 17] = FACCODRAZ;
                                objHoja.Cells[i, 19] = "D";
                                objHoja.Cells[i, 20] = Divisa.InnerText;
                                i ++;
                            }

                            /* Ultima linea de datos, por cada XML procesado */
                            objHoja.Cells[i, 1] = "FCD";
                            objHoja.Cells[i, 3] = ContadorProgreso;
                            objHoja.Cells[i, 5] = EmisionFactura.InnerText;
                            objHoja.Cells[i, 6] = EmisionFactura.InnerText;
                            objHoja.Cells[i, 10] = "N";
                            objHoja.Cells[i, 11] = NitEmisor.InnerText;
                            objHoja.Cells[i, 12] = RazonSocial.InnerText;
                            objHoja.Cells[i, 17] = FACCODRAZ;
                            objHoja.Cells[i, 19] = "C";
                            objHoja.Cells[i, 20] = Divisa.InnerText;
                            objHoja.Cells[i, 8] = "11050501";
                            objHoja.Cells[i, 18] = TOTAL;
                            objHoja.Cells[i, 21] = TOTAL;
                            i++;

                            /* Barra de porcentaje en la ventana */
                            double Progreso = 100/ProgresoTotal;
                            int PorcentajeInt = (int)Math.Round(Progreso,0);
                            pBarProceso.Value = pBarProceso.Value + PorcentajeInt;
                            if (ContadorProgreso == ProgresoTotal)
                            {
                                while (pBarProceso.Value < 100)
                                {
                                    pBarProceso.Value = pBarProceso.Value + 1;
                                }
                                MessageBox.Show("El proceso a finalizado");
                                pBarProceso.Value = 0;
                            }
                            ContadorProgreso++;

                        }
                        /* Guardar el resultado en la ruta */
                        objLibro.SaveAs(ruta + "\\Resultado\\" + "Resultado" + ".xlsx");
                        objLibro.Close();
                        objAplicacion.Quit();

                        /* Cambia el mensaje del nombre del archivo */
                        NombreArchivoLabel.Text = "\n\n“Los errores no son fracasos, son señal de que lo estamos intentando.";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }    
}
