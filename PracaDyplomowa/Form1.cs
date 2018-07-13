using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace PracaDyplomowa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string wayToFile;
        float[,] systemDecyzyjny; 

        //metoda dla wczytania systemu
        float[,] LoadSystem(string wayToFile)
        {
            //wczytanie pliku do zmiennej linie
            var linie = System.IO.File.ReadAllLines(wayToFile);
            int iloscKolumn = 0;
            int iloscWierszy = 0;
            //usuwanie pustego znaku(spację), ubiezpieczenie przed błędnym wczytaniem tablicy
            var linia2 = linie[0].Trim();
            //jeśli program napotka pusty znak -> do następnej linji
            var liczby2 = linia2.Split(' ');
            iloscWierszy = linie.Length;
            iloscKolumn = liczby2.Length;   
            systemDecyzyjny = new float[iloscWierszy, iloscKolumn]; 
            //wczytywanie systemu decyzyjnego
            for(int i = 0; i<linie.Length; i++)
            {
                var linia = linie[i].Trim();
                var liczby = linia.Split(' ');
                for(int j =0; j<liczby.Length; j++)
                {
                    systemDecyzyjny[i, j] = float.Parse(liczby[j].Trim());
                }
            }

            return systemDecyzyjny;
        }
        private void buttonChose_Click(object sender, EventArgs e)
        {
            //ograniczenie wyboru tylko do plików tekstowych
            openFileDialogWay.Filter = "txt files(*.txt)|*txt";

            //Zatwierdzenie śćieżki czy anulowanie operacji
            if (openFileDialogWay.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            labelWay.Text = openFileDialogWay.FileName;
            wayToFile = openFileDialogWay.FileName;
            //wywołanie funkcji
            systemDecyzyjny = LoadSystem(wayToFile);
        }
        //metoda do generowaniamaski do systemu decyzyjnego


        float[,] GenerateMask(float[,] systemDecision)
        {
            float[,] mask = new float[systemDecision.GetLength(0), systemDecision.GetLength(1)];
            for(int i = 0; i<systemDecision.GetLength(0); i++)
            {
                for(int j=0; j<systemDecision.GetLength(1); j++)
                {
                    mask[i, j] = systemDecision[i,j];
                }
            }
            return mask;
        }

        int GenerujPokrycie(float[,] systemDecision, float[,] regula)
        {
            int pokrycieSystemu = 0;
            float[,] zbiorRegul = new float[regula.GetLength(0), regula.GetLength(1)];
            for (int i =0; i <regula.GetLength(1); i++)
            {
                zbiorRegul[0, i] = regula[0, i];
                zbiorRegul[1, i] = regula[0, i];
            }
            for(int i=0; i<systemDecision.GetLength(0);i++)
            {
                bool pokrywaSie = true;
                for(int kolumna =0; kolumna<regula.GetLength(1); kolumna++)
                {
                    zbiorRegul[2, kolumna] = systemDecision[i, Convert.ToInt32(regula[0, kolumna])];
                }
                for(int s=0; s<regula.GetLength(1);s++)
                {
                    if(zbiorRegul[1,s]!=zbiorRegul[2,s])
                    {
                        pokrywaSie = false;
                    }
                }
                if(pokrywaSie ==true)
                {
                    pokrycieSystemu++;
                }
            }

            return pokrycieSystemu;
        }
        //algorytm sekwencyjnego pokrywania
        //metoda zwraca napis, który zawiera zbiór reguł
        string SequentialCovering(float[,] systemDecision)
        {
            string reguly = "";
            int numerReguly = 1;
            float[,] mask = GenerateMask(systemDecision);
            for(int i=1; i < systemDecision.GetLength(1); i++)
            {
                //tablica regula bedzie przechowywac w 1 rzedzie numer atrybutu
                // w 2 - wartość tego atrybutu
                //w 3 - wartość z kolejnego wiersza do porównania
                float[,] regula = new float[3, i];
                //zmienna do przechowywania decyzji
                float decyzjaReguly;
                //petla przeprowadza przez każdy wiersz, czyli przez każdy objekt systemu decyzyjnego
                for (int row = 0; row < systemDecision.GetLength(0); row++)
                {
                    //tworzenie dobierania pierwszych atrybutów
                    for(int fisrtAtribut = 0; fisrtAtribut<systemDecision.GetLength(1)-i; fisrtAtribut++)
                    {
                        //odebranie ostatniego atrybutu do reguły
                        for(int columns = fisrtAtribut; columns<systemDecision.GetLength(1)-i; columns++)
                        {
                            decyzjaReguly = mask[row, systemDecision.GetLength(1) - 1];
                            //zmienne pomocnicze
                            bool brakRegul = false;
                            int column = columns;
                            int liczbaX = 1000000000;
                            //dobieranie regul

                            if(mask[row,columns] !=liczbaX)
                            {
                                int whatAtribut = fisrtAtribut;
                                for(int j=0; j<i-1; j++)
                                {
                                    while(mask[row,whatAtribut]==liczbaX)
                                    {
                                        whatAtribut++;
                                    }
                                    //zabiezpieczenie, w razie zakonczenia przechodzenia wiersza
                                    //ale nie zostali dobrane deskryptory
                                    //w takim wypadku kończymy dziłanie pętli
                                    if(whatAtribut >= systemDecision.GetLength(1)-1)
                                    {
                                        j = i;
                                        brakRegul = true;
                                    }
                                    //else dla przypadku gdy zostaje stwożóna regula
                                    //zapisywanie
                                    else
                                    {
                                        regula[0, j] = whatAtribut;
                                        regula[1, j] = mask[row, whatAtribut];
                                        whatAtribut++;
                                    }
                                }
                                regula[0, i - 1] = columns;
                                regula[1, i - 1] = mask[row, columns];
                            }
                            else
                            {
                                brakRegul = true;
                                columns = systemDecision.GetLength(1);
                            }
                            //czy warunek brakRegul został spełniony
                            //jezeli nie został spełniony możemy przejść dalej
                            if (brakRegul==false)
                            {
                                //pomocnicze zmienne
                                bool sprzecznaRegula = false;
                                //sprawdzenie reguły
                                for(int nextRows = 0; nextRows<systemDecision.GetLength(0); nextRows++)
                                {
                                    bool nextRow = false;
                                    if(nextRows!=row)
                                    {
                                        //uzupelnienie ostatniego wiersza reguły, według kótrego
                                        //będzie sprawdzano poprawność reguły
                                        for(int uzupenienie =0; uzupenienie<i; uzupenienie++)
                                        {
                                            int column2 = Convert.ToInt32(regula[0, uzupenienie]);
                                            regula[2, uzupenienie] = systemDecision[nextRows, column2];
                                        }
                                        if (nextRow == false)
                                        {
                                            bool identycznyDeskryptor = true;
                                            for (int k = 0; k < i; k++)
                                            {
                                                //sprawdzanie czy wartośći deskryptorów takie same      
                                                if (regula[1, k] != regula[2, k])
                                                {
                                                    identycznyDeskryptor = false;
                                                    k = i;
                                                }
                                            }
                                            //gdyż deskryptory są identyczne -> porównuję decyzję
                                            if (identycznyDeskryptor == true && decyzjaReguly != systemDecision[nextRows, systemDecision.GetLength(1) - 1])
                                            {
                                                //gdy deskryptory są jednakowy, a decyzja !=, oznacza to, że reguła jest sprzeczna
                                                sprzecznaRegula = true;
                                            }
                                        }
                                    }
                                }
                                //wypisanie reguły 
                                if(sprzecznaRegula==false)
                                {
                                    int pokrycie = 0;
                                    reguly = reguly + 'R' + numerReguly + ":";
                                    for(int s=0;s<i;s++)
                                    {
                                        //konwertowanie wartości adresu reguły
                                        int numberAtribut = Convert.ToInt32(regula[0, s]) + i;
                                        reguly = reguly + 'a' + numberAtribut + '=' + Convert.ToString(regula[1, s]) + ' ';
                                        if(s<i-1)
                                        {
                                            reguly = reguly + '/' +@"\";
                                        }
                                    }
                                    reguly = reguly + "d=>" + decyzjaReguly;
                                    for(int rowMask = 0; rowMask<systemDecision.GetLength(0); rowMask++)
                                    {
                                        bool nextRow = false;
                                        for(int uzupelnienie=0; uzupelnienie<i; uzupelnienie++)
                                        {
                                            int columna2 = Convert.ToInt32(regula[0, uzupelnienie]);
                                            if(mask[rowMask, columna2] !=liczbaX)
                                            {
                                                regula[2, uzupelnienie] = mask[rowMask, columna2];
                                            }
                                            else
                                            {
                                                nextRow = true;
                                                uzupelnienie = i;
                                            }
                                        }
                                        //porównanie wartości
                                        for(int sprawdzenie=0; sprawdzenie<i; sprawdzenie++)
                                        {
                                            if(regula[1, sprawdzenie]!=regula[2,sprawdzenie])
                                            {
                                                //jezeli wartości się różnią przechodzi do kolejnego wiersza
                                                nextRow = true;
                                            }
                                        }
                                        if (nextRow == false)
                                        {
                                            for(int g=0; g<systemDecision.GetLength(1)-1;g++)
                                            {
                                                mask[rowMask, g] = liczbaX;
                                            }
                                        }
                                    }
                                    //ustawienie wartości pokrycia
                                    //pokrycie - do ilu obiektów pasuje reguła
                                    pokrycie = GenerujPokrycie(systemDecision, regula);
                                    if(pokrycie>1)
                                    {
                                        reguly = reguly + " [" + pokrycie + ']' + "\r\n";
                                    }
                                    else
                                    {
                                        reguly = reguly + "\r\n";
                                    }
                                    numerReguly++;
                                }
                            }
                        }
                    }
                }
            }
            return reguly;
        }
        string Exhaustive(float[,] systemDecision)
        {
            string reguly = "";
            float[,] maska = GenerateMask(systemDecision);
            int numerReguly = 1;
            //Przechowanie prawidłowej reguły, która będzie pobierana z listy reguł
            float[] tmpRegula = new float[systemDecision.GetLength(1)];
            var listaRegul = new List<float[]>();

            for(int i =1; i<systemDecision.GetLength(1); i++)
            {
                float[,] regula = new float[3, i];
                float decyzjaReguly;
                for(int row = 0; row<systemDecision.GetLength(0); row++)
                {
                    for (int firstTakenAtributes = 0; firstTakenAtributes<systemDecision.GetLength(1)-1; firstTakenAtributes++) 
                    {
                        for(int kolumny = firstTakenAtributes; kolumny<systemDecision.GetLength(1)-1; kolumny++)
                        {
                            decyzjaReguly = maska[row, systemDecision.GetLength(1) - 1];
                            bool brakRegul = false;
                            int kolumna = kolumny;
                            int liczbaX = 1000000000;
                            if (maska[row, kolumny] != liczbaX)
                            {
                                int wichAtribute = firstTakenAtributes;
                                for(int j=0; j<i-1; j++)
                                {
                                    while(maska[row, wichAtribute]==liczbaX)
                                    {
                                        wichAtribute++;
                                    }
                                    if(wichAtribute>=systemDecision.GetLength(1)-1)
                                    {
                                        j = i;
                                        brakRegul = true;
                                    }
                                    else
                                    {
                                        regula[0, j] = wichAtribute;
                                        regula[1, j] = maska[row, wichAtribute];
                                        wichAtribute++;
                                    }
                                }
                                regula[0, i - 1] = kolumny;
                                regula[1, i - 1] = maska[row, kolumny];
                            }
                            else
                            {
                                brakRegul = true;
                                kolumny = systemDecision.GetLength(1);
                            }
                            if(brakRegul==false)
                            {
                            //sprawdzamy czy znałeziona reguła nie jest sprzeczna
                            //czy przypadkiem nie zawiera reguły nizszego rzędu
                                bool sprzecznaRegula = false;
                                for (int nextRows =0; nextRows<systemDecision.GetLength(0); nextRows++)
                                {
                                //sprawdzenie niedokonujemy dla wiersza, 
                                //z którego pochodzi potencjalna reguła
                                    if(nextRows != row)
                                    {
                                        for (int uzupelnienie =0; uzupelnienie<i; uzupelnienie++)
                                        {
                                            int kolumna2 = Convert.ToInt32(regula[0, uzupelnienie]);
                                            regula[2, uzupelnienie] = systemDecision[nextRows, kolumna2];
                                        }
                                        bool identycznyDeskryptor = true;
                                        for (int k=0; k<1;k++)
                                        {
                                            //w tej pętli sprawdzam czy deskryptory potencjalnej reguły
                                            //są identyczne z deskryptorami sprawdzanymi   
                                            if (regula[1,k]!=regula[2,k])
                                            {
                                                identycznyDeskryptor = false;
                                                k = i;
                                            }
                                        }
                                        if(identycznyDeskryptor==true && decyzjaReguly!=systemDecision[nextRows, systemDecision.GetLength(1)-1])
                                        {
                                            sprzecznaRegula = true;
                                        }
                                    }
                                }
                                //sprawdzenie, czy potencjalna regula zawiera w sobie pełną regulę niższego rzędu
                                //tu rzechodzimy tylko wtedy gdy regula nie jest sprzeczna
                                //oraz reguła jest przynajmniej drugiego rzędu
                                if(sprzecznaRegula==false && i>1)
                                {
                                    //porównanie potencjalnej reguły z każdą do tychczas utworzoną regułą
                                    //odwołanie do listy reguł
                                    int rozmiarListy = listaRegul.Count;
                                    float[,] pomocniczaTablica = new float[2, systemDecision.GetLength(1)];
                                    for(int uzupelnienie=0; uzupelnienie<pomocniczaTablica.GetLength(1); uzupelnienie++)
                                    {
                                        //wypelniamy pierwszy wiersz wartosciami -1
                                        pomocniczaTablica[0, uzupelnienie] = -1;
                                    }
                                    for (int uzupelnienie = 0; uzupelnienie < i; uzupelnienie++)
                                    {
                                        int numerAtrybutu = Convert.ToInt32(regula[0, uzupelnienie]);
                                        pomocniczaTablica[0, numerAtrybutu] = -2;
                                        //do pomocniczej tablicy nie wypisuję reguł
                                        //interesuję tylko atrybut na którym znajdują się reguły
                                        //-1 oznacza miejsce w kótym niema reguły
                                        //-2 oznacza miejsce w którym występuje reguła
                                    }
                                    //przecodzimy przez każdą tablicę reguł dodanych do listy
                                    for(int atrybutListy=0; atrybutListy<rozmiarListy; atrybutListy++)
                                    {
                                        bool wystepujeNaTychSamychAtrybutach = true;
                                        tmpRegula = listaRegul[atrybutListy];
                                        for(int ktoraKolumna=0; ktoraKolumna<tmpRegula.Length - 1; ktoraKolumna++)
                                        {
                                            if (tmpRegula[ktoraKolumna]!=-1)
                                            {
                                                //regula jest
                                                pomocniczaTablica[1, ktoraKolumna] = -2;
                                            }
                                            else
                                            {
                                                //brak reguly
                                                pomocniczaTablica[1, ktoraKolumna] = 0;
                                            }
                                        }
                                        //czy wczesniejsze reguly znajduja sie na tych samych atrybutach co potencjalna regula
                                        for(int ktoraKolumna=0; ktoraKolumna<pomocniczaTablica.GetLength(1); ktoraKolumna++)
                                        {
                                            if(pomocniczaTablica[0, ktoraKolumna]==-1 && pomocniczaTablica[1, ktoraKolumna]==-2)
                                            {
                                            wystepujeNaTychSamychAtrybutach = false;
                                            }
                                        }
                                        if (wystepujeNaTychSamychAtrybutach == true)
                                        {
                                            for(int atrybut =0; atrybut<i; atrybut++)
                                            {
                                                int numerAtybutu = Convert.ToInt32(regula[0, atrybut]);
                                                regula[2, atrybut] = tmpRegula[numerAtybutu];
                                            }
                                            bool identyczneDeskryptory = true;
                                            for(int atrybut =0; atrybut<i; atrybut++)
                                            {
                                            //spradzenie, czy utwożona reguła rożni się o wcześniej utworzonej reguły
                                            //i czy ta różnica nie wynika z pojawienia się -1 w 2 regulie
                                            //wtedy oznacza, ze to regula nizszego rzedu
                                                if(regula[1,atrybut]!=regula[2,atrybut]&&regula[2,atrybut]!=-1)
                                                {
                                                    identyczneDeskryptory = false;
                                                }
                                            }
                                            //Jezeli okaże się że deskryptory są jednakowej wartości
                                            //oznacza to że reguła jest sprzeczna
                                            //poniewarz zawiera w sobie reguly nizszego rzedu.
                                            if(identyczneDeskryptory=true)
                                            {
                                                sprzecznaRegula = true;
                                            }
                                        }
                                    }
                                }
                                if(sprzecznaRegula==false)
                                {
                                    float[] pomocniczaTablica = new float[tmpRegula.Length];
                                    for(int numerKolumny=0; numerKolumny<tmpRegula.Length;numerKolumny++)
                                    {
                                        pomocniczaTablica[numerKolumny] = -1;
                                    }
                                    int pokrycie = 0;
                                    reguly = reguly + 'R' + numerReguly + ':';
                                    for(int kol=0; kol<i; kol++)
                                    {
                                        int numerAtrybutu = Convert.ToInt32(regula[0, kol]) + 1;
                                        reguly = reguly + " a" + numerAtrybutu + '=' + Convert.ToString(regula[1, kol]) + ' ';
                                        pomocniczaTablica[numerAtrybutu - 1] = regula[1, kol];
                                        if(kol<i-1)
                                        {
                                            reguly = reguly + '/' + @"\";
                                        }
                                    }
                                    //dodawanie decyzji
                                    reguly = reguly + "d=>" + decyzjaReguly;
                                    pomocniczaTablica[tmpRegula.Length - 1] = decyzjaReguly;
                                    listaRegul.Add(pomocniczaTablica);
                                    //zakrycie maski systemu decyzyjnego
                                    if(i==1)
                                    {
                                        for(int wierszMaski=0; wierszMaski<systemDecision.GetLength(0); wierszMaski++)
                                        {
                                            bool kolejnyWiersz = false;
                                            for(int uzupelnianie =0; uzupelnianie<i; uzupelnianie++)
                                            {
                                                int kolumna2 = Convert.ToInt32(regula[0, uzupelnianie]);
                                                if(maska[wierszMaski, kolumna2]!=liczbaX)
                                                {
                                                    regula[2, uzupelnianie] = maska[wierszMaski, kolumna2];
                                                }
                                                else
                                                {
                                                    kolejnyWiersz = true;
                                                    uzupelnianie = i;
                                                }
                                            }
                                            for(int sprawdzenie=0; sprawdzenie<i; sprawdzenie++)
                                            {
                                                int kolumna2 = Convert.ToInt32(regula[0, sprawdzenie]);
                                                if (regula[1,sprawdzenie] !=regula[2, sprawdzenie])
                                                {
                                                    kolejnyWiersz = true;
                                                  
                                                }
                                                //zakrywanie tylko jednego deskryptore
                                                if(kolejnyWiersz==false)
                                                {
                                                    maska[wierszMaski, kolumna2] = liczbaX;
                                                }
                                            }
                                        }
                                    }
                                    //uzupelnienie pokrycia
                                    pokrycie = GenerujPokrycie(systemDecision, regula);
                                    if(pokrycie>1)
                                    {
                                        reguly = reguly + " [" + pokrycie + ']' + "\r\n";
                                    }
                                    //pokrycie jest dodawane tylko jezeli jest >1
                                    //jezeli pokrycie nie jest >1 -> przejscie do kolejnej linijki
                                    else
                                    {
                                        reguly = reguly + "\r\n";
                                    }
                                    numerReguly++;
                                }
                            }
                        }
                    }
                }
            }
            return reguly;
        }

        string LearnFromExampelByModules(float[,] systemDecyzyjny)
        {
            string reguly = " ";
            var listaDecyzji = new List<float>();
            for (int i = 0; i < systemDecyzyjny.GetLength(0); i++)
            {
                bool wystepujeWLiscie = false;
                for (int wystepowanie = 0; wystepowanie < listaDecyzji.Count; wystepowanie++)
                {
                    if (systemDecyzyjny[i, systemDecyzyjny.GetLength(1) - 1] == listaDecyzji[wystepowanie])
                    {
                        wystepujeWLiscie = true;
                    }
                }
                if (wystepujeWLiscie == false)
                {
                    listaDecyzji.Add(systemDecyzyjny[i, systemDecyzyjny.GetLength(1) - 1]);
                }
            }
            var listaRegul = new List<float[]>();
            int numerReguly = 1;
            for (int i = 0; i < listaDecyzji.Count; i++)
            {
                float[,] maska = GenerateMask(systemDecyzyjny);
                for (int wiersz = 0; wiersz < systemDecyzyjny.GetLength(0); wiersz++)
                {
                    if (systemDecyzyjny[wiersz, systemDecyzyjny.GetLength(1) - 1] != listaDecyzji[i])
                    {
                        for (int kolumna = 0; kolumna < systemDecyzyjny.GetLength(1); kolumna++)
                        {
                            maska[wiersz, kolumna] = 999;
                        }
                    }
                }
                bool pokrytyCalyKoncept = false;
                while (pokrytyCalyKoncept == false)
                {
                    float[,] tablicaPowtorzen = new float[3, 2];
                    float[,] tablicaPorownaniaReguly = new float[2, systemDecyzyjny.GetLength(1)];
                    for (int uzupelnenie = 0; uzupelnenie < systemDecyzyjny.GetLength(1); uzupelnenie++)
                    {
                        tablicaPorownaniaReguly[0, uzupelnenie] = -1;
                        tablicaPorownaniaReguly[1, uzupelnenie] = -1;
                    }
                    pokrytyCalyKoncept = true;
                    bool regulaSprzeczna = true;
                    reguly = reguly + 'R' + numerReguly + ':';
                    while (regulaSprzeczna == true)
                    {
                        int wierszNajczestrzegoPowtorzenia = 0;
                        for (int pierwszyWiersz = 0; pierwszyWiersz < systemDecyzyjny.GetLength(0); pierwszyWiersz++)
                        {
                            for (int kol = 0; kol < systemDecyzyjny.GetLength(1) - 1; kol++)
                            {
                                if (maska[pierwszyWiersz, 0] == 999)
                                {
                                    kol = systemDecyzyjny.GetLength(1);
                                }
                                if (maska[pierwszyWiersz, 0] != 999 && tablicaPorownaniaReguly[0, kol] == -1)
                                {
                                    tablicaPowtorzen[0, 0] = kol;
                                    tablicaPowtorzen[1, 0] = maska[pierwszyWiersz, kol];
                                    int powtorzeniaDeskryptora = 0;
                                    for (int powtorzenia = 0; powtorzenia < systemDecyzyjny.GetLength(0); powtorzenia++)
                                    {
                                        if (tablicaPowtorzen[1, 0] == maska[powtorzenia, kol])
                                        {
                                            powtorzeniaDeskryptora++;
                                        }
                                    }
                                    tablicaPowtorzen[2, 0] = powtorzeniaDeskryptora;
                                    wierszNajczestrzegoPowtorzenia = pierwszyWiersz;
                                    pierwszyWiersz = systemDecyzyjny.GetLength(0);
                                    kol = systemDecyzyjny.GetLength(1);
                                }
                            }
                        }
                        for (int kolumna = 0; kolumna < systemDecyzyjny.GetLength(1) - 1; kolumna++)
                        {
                            if (tablicaPorownaniaReguly[0, kolumna] == -1)
                            {
                                for (int wiersz = 0; wiersz < systemDecyzyjny.GetLength(0); wiersz++)
                                {
                                    if (maska[wiersz, kolumna] != 999)
                                    {
                                        tablicaPowtorzen[0, 1] = kolumna;
                                        tablicaPowtorzen[1, 1] = maska[wiersz, kolumna];
                                        int powtorzeniaDeskryptora = 0;
                                        for (int powtorzenia = 0; powtorzenia < systemDecyzyjny.GetLength(0); powtorzenia++)
                                        {
                                            if (tablicaPowtorzen[1, 1] == maska[powtorzenia, kolumna])
                                            {
                                                powtorzeniaDeskryptora++;
                                            }
                                        }
                                        tablicaPowtorzen[2, 1] = powtorzeniaDeskryptora;
                                        if (tablicaPowtorzen[2, 1] > tablicaPowtorzen[2, 0])
                                        {
                                            tablicaPowtorzen[0, 0] = tablicaPowtorzen[0, 1];
                                            tablicaPowtorzen[1, 0] = tablicaPowtorzen[1, 1];
                                            tablicaPowtorzen[2, 0] = tablicaPowtorzen[2, 1];
                                        }
                                    }
                                }
                            }
                        }
                        int tmpNumerAtrybutu = Convert.ToInt32(tablicaPowtorzen[0, 0]) + 1;
                        reguly = reguly + " a" + tmpNumerAtrybutu + '=' + tablicaPowtorzen[1, 0] + ' ';
                        int numerAtrybutu = Convert.ToInt32(tablicaPowtorzen[0, 0]);
                        tablicaPorownaniaReguly[0, numerAtrybutu] = tablicaPowtorzen[1, 0];
                        tablicaPorownaniaReguly[0, systemDecyzyjny.GetLength(1) - 1] = systemDecyzyjny[wierszNajczestrzegoPowtorzenia, systemDecyzyjny.GetLength(1) - 1];
                        for (int wierszSprawdzajacy = 0; wierszSprawdzajacy < systemDecyzyjny.GetLength(0); wierszSprawdzajacy++)
                        {
                            for (int uzupelnienie = 0; uzupelnienie < systemDecyzyjny.GetLength(1); uzupelnienie++)
                            {
                                if (tablicaPorownaniaReguly[0, uzupelnienie] != -1)
                                {
                                    tablicaPorownaniaReguly[1, uzupelnienie] = systemDecyzyjny[wierszSprawdzajacy, uzupelnienie];
                                }
                            }
                            tablicaPorownaniaReguly[1, systemDecyzyjny.GetLength(1) - 1] = systemDecyzyjny[wierszSprawdzajacy, systemDecyzyjny.GetLength(1) - 1];
                            bool identyczneAtrybuty = true;
                            for (int sprawdzenieAtrybutow = 0; sprawdzenieAtrybutow < systemDecyzyjny.GetLength(1) - 1; sprawdzenieAtrybutow++)
                            {
                                if (tablicaPorownaniaReguly[0, sprawdzenieAtrybutow] != tablicaPorownaniaReguly[1, sprawdzenieAtrybutow])
                                {
                                    identyczneAtrybuty = false;
                                    regulaSprzeczna = false;
                                    sprawdzenieAtrybutow = systemDecyzyjny.GetLength(1);
                                }
                            }
                            if (identyczneAtrybuty == true && tablicaPorownaniaReguly[0, systemDecyzyjny.GetLength(1) - 1] != tablicaPorownaniaReguly[1, systemDecyzyjny.GetLength(1) - 1])
                            {
                                regulaSprzeczna = true;
                                wierszSprawdzajacy = systemDecyzyjny.GetLength(0);
                            }
                            else
                            {
                                regulaSprzeczna = false;
                            }
                        }
                        if (regulaSprzeczna == true)
                        {
                            for (int obiekt = 0; obiekt < systemDecyzyjny.GetLength(0); obiekt++)
                            {
                                if (maska[obiekt, numerAtrybutu] != tablicaPowtorzen[1, 0] && maska[obiekt, numerAtrybutu] != 999)
                                {
                                    for (int kol = 0; kol < systemDecyzyjny.GetLength(1); kol++)
                                    {
                                        maska[obiekt, kol] = 999;
                                    }
                                }
                            }
                            reguly = reguly + '/' + @"\";
                        }
                        if (regulaSprzeczna == false)
                        {
                            int dlugoscReguly = 0;
                            for (int licznik = 0; licznik < systemDecyzyjny.GetLength(1) - 1; licznik++)
                            {
                                if (tablicaPorownaniaReguly[0, licznik] != -1)
                                {
                                    dlugoscReguly++;
                                }
                            }
                            float[,] tmpTablica = new float[3, dlugoscReguly];
                            int tmpLicznik = 0;
                            for (int uzupelnienie = 0; uzupelnienie < systemDecyzyjny.GetLength(1) - 1; uzupelnienie++)
                            {
                                if (tablicaPorownaniaReguly[0, uzupelnienie] != -1)
                                {
                                    tmpTablica[0, tmpLicznik] = uzupelnienie;
                                    tmpTablica[1, tmpLicznik] = tablicaPorownaniaReguly[0, uzupelnienie];
                                    tmpLicznik++;
                                }
                            }
                            int pokrycie = GenerujPokrycie(systemDecyzyjny, tmpTablica);
                            reguly = reguly + "d=>" + tablicaPorownaniaReguly[0, systemDecyzyjny.GetLength(1) - 1];
                            if (pokrycie > 1)
                            {
                                reguly = reguly + " [" + pokrycie + ']' + "\r\n";
                            }
                            else
                            {
                                reguly = reguly + "\r\n";
                            }
                        }
                    }
                    numerReguly++;
                    float[] regula = new float[systemDecyzyjny.GetLength(1)];
                    for (int przepisanie = 0; przepisanie < systemDecyzyjny.GetLength(1); przepisanie++)
                    {
                        regula[przepisanie] = tablicaPorownaniaReguly[0, przepisanie];
                    }
                    listaRegul.Add(regula);
                    maska = GenerateMask(systemDecyzyjny);
                    for (int wiersz = 0; wiersz < systemDecyzyjny.GetLength(0); wiersz++)
                    {
                        if (systemDecyzyjny[wiersz, systemDecyzyjny.GetLength(1) - 1] != listaDecyzji[i])
                        {
                            for (int kolumna = 0; kolumna < systemDecyzyjny.GetLength(1); kolumna++)
                            {
                                maska[wiersz, kolumna] = 999;
                            }
                        }
                    }
                    for (int zakrywanie = 0; zakrywanie < listaRegul.Count; zakrywanie++)
                    {
                        float[] tmpTablica = listaRegul[zakrywanie];
                        for (int sprObiekt = 0; sprObiekt < systemDecyzyjny.GetLength(0); sprObiekt++)
                        {
                            bool identycznyObiekt = true;
                            for (int sprawdzenie = 0; sprawdzenie < systemDecyzyjny.GetLength(1); sprawdzenie++)
                            {
                                if (tmpTablica[sprawdzenie] != maska[sprObiekt, sprawdzenie] && tmpTablica[sprawdzenie] != -1)
                                {
                                    identycznyObiekt = false;
                                    sprawdzenie = systemDecyzyjny.GetLength(1);
                                }
                            }
                            if (identycznyObiekt == true)
                            {
                                for (int atrybut = 0; atrybut < systemDecyzyjny.GetLength(1); atrybut++)
                                {
                                    maska[sprObiekt, atrybut] = 999;
                                }
                            }
                        }
                    }
                    for (int pokrytyKoncept = 0; pokrytyKoncept < systemDecyzyjny.GetLength(0); pokrytyKoncept++)
                    {
                        if (maska[pokrytyKoncept, 0] != 999)
                        {
                            pokrytyCalyKoncept = false;
                            pokrytyKoncept = systemDecyzyjny.GetLength(0);
                        }
                    }
                }
            }
            return reguly;
        }
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            // przechowywanie zbioru regul
            string regulySeqCov = SequentialCovering(systemDecyzyjny);
            string regulyExhaustive = Exhaustive(systemDecyzyjny);
            string regulyLEM = LearnFromExampelByModules(systemDecyzyjny);
            lable_reguly.Text = regulyLEM;
            label1.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string regulyLEM = LearnFromExampelByModules(systemDecyzyjny);
            File.WriteAllText("reguly.txt", regulyLEM);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lable_reguly_Click(object sender, EventArgs e)
        {

        }

        private void labelWay_Click(object sender, EventArgs e)
        {

        }
    }
}
 