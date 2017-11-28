namespace Checkout.BlackPanther.CardProcessing.Model
{
    /*
        GatewayActionCode	Description
        1	                Purchase
        2	                Refund
        3	                Void Purchase
        4	                Authorisation
        5	                Capture
        7	                Void Capture
        8	                Query
        9	                Void Authorisation
        14	                Original Credit (Payout)
        15	                Void Original Credit
        19	                Void Authorisation and Blacklist
        17	                Expiry
        20	                Account Verfication
        50	                Chargeback
    */

    public enum GatewayActionCode
    {
        Refund = 2,
        Authorise = 4,
        Capture = 5,
        VoidAuthorization = 9,
        Payout = 14,
        AccountVerification = 20
    }
}
