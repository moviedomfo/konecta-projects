using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.DirectoryServices;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace EpironAPI.classes
{
    public class ActiveDirectory
    {
        private string _path;
        //private string _filterAttribute;

        private string _descripcion;

        public void AutenticarLDAP(string path)
        {
            _path = path;
        }

        public string isDisplayName
        {
            get { return _descripcion; }
        }

        public string Devuelve_Propiedad(string Domain, string username, string pwd, string Propiedad)
        {
            string domainAndUsername = (Domain + "\\") + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=user)(anr=" + username + "))";
                SearchResult resEnt = search.FindOne();
                DirectoryEntry de = resEnt.GetDirectoryEntry();

                _descripcion = de.Properties[Propiedad].Value.ToString();


                entry.Close();
            }
            catch (Exception ex)
            {
                return "Error al traer la informacion " + ex.ToString();
            }

            return _descripcion;
        }


        public bool checkUser(string Domain, string username, string pwd)
        {
            string sDomain = "LDAP://172.22.14.40/";

            string sDefaultOU = "ou=users,ou=system";

            string sServiceUser = @"uid=admin,ou=system";
            string sServicePassword = "secret";

            PrincipalContext oPrincipalContext = new PrincipalContext
               (ContextType.Domain, sDomain, sDefaultOU, sServiceUser, sServicePassword);

            UserPrincipal usr = UserPrincipal.FindByIdentity(oPrincipalContext,
                                           IdentityType.SamAccountName,
                                           "pnunez");

            if (usr != null)
            {
                if (usr.Enabled == false)
                    usr.Enabled = true;

                usr.Save();
                usr.Dispose();
            }
            oPrincipalContext.Dispose();


            return true;

        }
        public bool IsAuthenticated(string Domain, string username, string pwd)
        {
            bool Success = false;
            DirectoryEntry Entry = new DirectoryEntry(Domain, username, pwd);
            DirectorySearcher Searcher = new DirectorySearcher(Entry);
            Searcher.SearchScope = SearchScope.OneLevel;
            try
            {

                System.DirectoryServices.SearchResult Results = Searcher.FindOne();
                Success = (Results != null);
            }
            catch
            {
                Success = false;
            }
            return Success;
        }

        //public  bool CheckUserExistence(string pLdapPath, string pUserName, string pUserPwd)
        //{
        //    try
        //    {
        //        DirectoryEntry wEntry = new DirectoryEntry(pLdapPath, pUserName, pUserPwd);
        //        DirectorySearcher wSearch = new DirectorySearcher(wEntry);
        //        wSearch.Filter = "(SAMAccountName=" + pUserName + ")";
        //        SearchResult wResult = wSearch.FindOne();
        //        if (wResult == null)
        //            return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public DataTable GetGroups(string Domain, string rootUsername, string rootpwd, string username)
        {
            DataTable dtGroupNames = new DataTable();
            // Creamos un objeto DirectoryEntry para conectarnos al directorio activo
            DirectoryEntry adsRoot = new DirectoryEntry(Domain, rootUsername, rootpwd);
            // Creamos un objeto DirectorySearcher para hacer una búsqueda en el directorio activo
            DirectorySearcher adsSearch = new DirectorySearcher(adsRoot);
            DataRow row;
            try
            {
                // Ponemos como filtro que busque el usuario actual
                adsSearch.Filter = "samAccountName=" + username;

                // Extraemos la primera coincidencia
                SearchResult oResult;
                oResult = adsSearch.FindOne();

                // Obtenemos el objeto de ese usuario
                DirectoryEntry usuario = oResult.GetDirectoryEntry();

                // Obtenemos la lista de SID de los grupos a los que pertenece
                usuario.RefreshCache(new string[] { "tokenGroups" });

                // Creamos una variable StringBuilder donde ir añadiendo los SID para crear un filtro de búsqueda
                StringBuilder sids = new StringBuilder();
                sids.Append("(|");
                foreach (byte[] sid in usuario.Properties["tokenGroups"])
                {
                    sids.Append("(objectSid=");
                    for (int indice = 0; indice < sid.Length; indice++)
                    {
                        sids.AppendFormat("\\{0}", sid[indice].ToString("X2"));
                    }
                    sids.AppendFormat(")");
                }
                sids.Append(")");

                // Creamos un objeto DirectorySearcher con el filtro antes generado y buscamos todas la coincidencias
                DirectorySearcher ds = new DirectorySearcher(adsRoot, sids.ToString());
                SearchResultCollection src = ds.FindAll();
                if (src.Count > 0)
                {
                    dtGroupNames.Columns.Add("Grupo", typeof(string));
                }

                // Recorremos toda la lista de grupos devueltos
                foreach (SearchResult sr in src)
                {
                    String sGrupo = (String)sr.Properties["samAccountName"][0];
                    // A partir de aquí hacer lo que corresponda con cada grupo
                    //lo guardaria en un datatable
                    row = dtGroupNames.NewRow();
                    row["Grupo"] = sGrupo;
                    dtGroupNames.Rows.Add(row);
                }

            }
            catch (Exception)
            {
                //groupNames = ex.Message;
            }
            return dtGroupNames;
        }

    }
}