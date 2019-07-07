namespace Trine.Mobile.Bll.Impl.Messages
{
    public class ErrorMessages
    {
        public static readonly string error = "Erreur";

        // Account
        public static readonly string uknownUserHeaderText = "Utilisateur inconnu";
        public static readonly string unknownUserBodyText = "Veuillez vérifier votre mail / mot de passe.";
        public static readonly string companyAlreadyExists = "Cette société existe déjà. Vous pouvez demander à la rejoindre.";
        public static readonly string userAlreadyExists = "Un utilisateur est déjà renseigné avec cet email. Veuillez-vous connectez, ou utiliser un autre email.";

        // Missions
        public static readonly string tripartiteMissionNeedsAtLeastThreeUsers = "Une mission tripartite nécessite un commercial, un client et un consultant.";
        public static readonly string directMissionNeedsAtLeastTwoUsers = "Une mission en direct nécessite un un client et un consultant.";
        public static readonly string acceptConditions = "Vous devez lire et accepter les conditions générales d'utilisation avant de créer un contrat de mission.";

        // Buttons
        public static readonly string okButtonText = "Ok";

        // Technical
        public static readonly string serverErrorText = "An error occured while requesting the server. Check your connectivity, or retry later. Error code : ";
        public static readonly string unknownError = "Une erreur inconnue s'est produite";
        public static readonly string errorOccuredGettingCardComponent = "An error occured while getting the card component";
        public static readonly string RandomNetworkErrorMessage = "Une erreur technique vient de se produire, vérifiez votre connectivité et recommencez. Si le problème subsiste, soyez sûrs que nous sommes déjà en train de travailler à le résoudre !";
        public static readonly string GetInviteError = "Une erreur s'est produite lors de la récupération d'une invitation. Veuillez rééssayer plus tard.";
        public static readonly string organizationAtLeastOneMember = "An organization has to have at least one member";
        public static readonly string activityConsultantHasToSignFirst = "Le consultant doit signer avant le client.";
    }
}
