using System;


public class ConvertisseurChiffresMot{

    // selon les régles décrites sur le site de l'académie francaise
    // http://www.academie-francaise.fr/la-langue-francaise/questions-de-langue#57_strong-em-nombres-criture-lecture-accord-em-strong

    public static string convertir(float chiffre){
			int centaine, dizaine, unite, reste, y;
		    bool dix = false;
			string lettre = "";
 
		    reste = (int)chiffre / 1;
 
		    for(int i=1000000000; i>=1; i/=1000){
		        y = reste/i;
		        if(y!=0){
		            centaine = y/100;
		            dizaine  = (y - centaine*100)/10;
		            unite = y-(centaine*100)-(dizaine*10);

                    lettre = Convert_centaine(centaine, dizaine, unite, lettre);
                    string[] DIZAINE = Convert_dizaine(dix, dizaine, unite, lettre);
                    lettre = DIZAINE[1];
                    dix = Convert.ToBoolean(DIZAINE[0]);
                    lettre = Convert_dizaine( dix,  unite,  lettre); 
  
		            switch (i)
		            {
		                case 1000000000:
		                    if(y>1) lettre+="milliards ";
		                    else lettre+="milliard ";
		                    break;
		                case 1000000:
		                    if(y>1) lettre+="millions ";
		                    else lettre+="million ";
		                    break;
		                case 1000:
		                    lettre+="mille ";
                            break;
		            }
		        } // end if(y!=0)
		        reste -= y*i;
		        dix = false;
		    } // end for
		    if(lettre.Length ==0) lettre+="zero"; 
 
			return lettre;
    }

    private static string Convert_centaine(int centaine, int dizaine, int unite, string lettre){
        switch (centaine){
            /*case 0:
                return lettre;*/
            case 1:
                return lettre += "cent ";
            case 2:
                if ((dizaine == 0) && (unite == 0)) return lettre += "deux cents ";
                else return lettre += "deux cent ";
            case 3:
                if ((dizaine == 0) && (unite == 0)) return lettre += "trois cents ";
                else return lettre += "trois cent ";
            case 4:
                if ((dizaine == 0) && (unite == 0)) return lettre += "quatre cents ";
                else return lettre += "quatre cent ";
            case 5:
                if ((dizaine == 0) && (unite == 0)) return lettre += "cinq cents ";
                else return lettre += "cinq cent ";
            case 6:
                if ((dizaine == 0) && (unite == 0)) return lettre += "six cents ";
                else return lettre += "six cent ";
            case 7:
                if ((dizaine == 0) && (unite == 0)) return lettre += "sept cents ";
                else return lettre += "sept cent ";
            case 8:
                if ((dizaine == 0) && (unite == 0)) return lettre += "huit cents ";
                else return lettre += "huit cent ";
            case 9:
                if ((dizaine == 0) && (unite == 0)) return lettre += "neuf cents ";
                else return lettre += "neuf cent ";
            default:
                return lettre;
        }
    }

    private static string[] Convert_dizaine( bool dix, int dizaine, int unite, string lettre){
        string[] Convert_dix = new string[2];
        Convert_dix[0] = "False";
        switch (dizaine){
            /*case 0:
                Convert_dix[1] = lettre;
                break;*/
            case 1:
                Convert_dix[0] = "True";
                Convert_dix[1] = lettre;;
                break;
            case 2:
                if (unite == 1) Convert_dix[1] = lettre += "vingt et ";
                else Convert_dix[1] = lettre += "vingt ";
                break;
            case 3:
                if (unite == 1) Convert_dix[1] = lettre += "trente et ";
                else Convert_dix[1] = lettre += "trente ";
                break;
            case 4:
                if (unite == 1) Convert_dix[1] = lettre += "quarante et ";
                else Convert_dix[1] = lettre += "quarante ";
                break;
            case 5:
                if (unite == 1) Convert_dix[1] = lettre += "cinquante et ";
                else Convert_dix[1] = lettre += "cinquante ";
                break;
            case 6:
                if (unite == 1) Convert_dix[1] = lettre += "soixante et ";
                else Convert_dix[1] = lettre += "soixante ";
                break;
            case 7:
                Convert_dix[0] = "True";
                if (unite == 1) Convert_dix[1] = lettre += "soixante et ";
                else Convert_dix[1] = lettre += "soixante ";
                break;
            case 8:
                if (unite == 0) Convert_dix[1] = lettre += "quatre-vingts ";
                else Convert_dix[1] = lettre += "quatre-vingt ";
                break;
            case 9:
                Convert_dix[0] = "True";
                Convert_dix[1] = lettre += "quatre-vingt ";
                break;
            default:
                Convert_dix[1] = lettre;             
                break;
        }
        return Convert_dix;
    }

    private static string Convert_dizaine(bool dix, int unite, string lettre){
        switch (unite){
            /*case 0:
                if (dix) return lettre += "dix ";*/
            case 1:
                if (dix) return lettre += "onze ";
                else return lettre += "un ";
            case 2:
                if (dix) return lettre += "douze ";
                else return lettre += "deux ";
            case 3:
                if (dix) return lettre += "treize ";
                else return lettre += "trois ";
            case 4:
                if (dix) return lettre += "quatorze ";
                else return lettre += "quatre ";
            case 5:
                if (dix) return lettre += "quinze ";
                else return lettre += "cinq ";
            case 6:
                if (dix) return lettre += "seize ";
                else return lettre += "six ";
            case 7:
                if (dix) return lettre += "dix-sept ";
                else return lettre += "sept ";

            case 8:
                if (dix) return lettre += "dix-huit ";
                else return lettre += "huit ";
            case 9:
                if (dix) return lettre += "dix-neuf ";
                else return lettre += "neuf ";
            default:
                return lettre;
        }
    }
}

