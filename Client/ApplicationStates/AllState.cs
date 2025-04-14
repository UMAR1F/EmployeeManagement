namespace Client.ApplicationStates
{
    public class AllState
    { 
        //scope
        public Action? Action { get; set; }

        //GeneralDeparttment

        public bool ShowGeneralDepartment { get; set; }
        public void GeneralDepartMentCliked()
        {
            ResetAllDepartMents();
            ShowGeneralDepartment = true;
            Action?.Invoke();
        }

       

        public bool ShowDepartment { get; set; }
        public void DepartMentCliked()
        {
            ResetAllDepartMents();
            ShowGeneralDepartment = true;
            Action?.Invoke();
        }

       


        public bool ShowBranch { get; set; }
        public void BranchCliked()
        {
            ResetAllDepartMents();
            ShowBranch = true;
            Action?.Invoke();
        }
       


        public bool ShowCountry { get; set; }
        public void CountryCliked()
        {
            ResetAllDepartMents();

            ShowCountry = true;
            Action?.Invoke();
        }

       

        public bool ShowCity { get; set; }
        public void CityCliked()
        {
            ResetAllDepartMents();

            ShowCity = true;
            Action?.Invoke();
        }

      



        public bool ShowTown { get; set; }
        public void TownCliked()
        {
            ResetAllDepartMents();
            ShowCity = true;
            Action?.Invoke();
        }



        public bool ShowUser { get; set; }
        public void UserCliked()
        {
            ResetAllDepartMents();

            ShowCity = true;
            Action?.Invoke();
        }



        public bool ShowEmployees{ get; set; }
        public void EmpoloyessCliked()
        {
            ResetAllDepartMents();

            ShowCity = true;
            Action?.Invoke();
        }



        private void ResetAllDepartMents()
        {
            ShowGeneralDepartment = false;
            ShowBranch = false;
            ShowCity = false;
            ShowBranch = false;
            ShowCountry = false;
            ShowDepartment = false; 
            ShowEmployees=false;
            ShowUser = false;
            

        }


    }
}
