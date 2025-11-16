namespace ApocalypticFastFood.Services
{
    public interface ILoyaltyPointsCalculator
    {
        int CalculatePoints(DiscountManager dm);
    }

    public class DefaultLoyaltyPointsCalculator : ILoyaltyPointsCalculator
    {
        public int CalculatePoints(DiscountManager dm)
        {
            var points = 0;

            switch (dm.CustomerType)
            {
                case 1:
                    points = (int)(dm.TotalAmount * 1);
                    if (dm.HasApp)
                    {
                        if (dm.EmailSubscribed)
                        {
                            if (dm.VisitCount > 10)
                                points = (int)(dm.TotalAmount * 2);
                            else
                                points = (int)(dm.TotalAmount * 1.5);
                        }
                        else
                        {
                            points = (int)(dm.TotalAmount * 1.2);
                        }
                    }

                    break;
                case 2:
                    switch (dm.MembershipLevel)
                    {
                        case "Bronze":
                            points = (int)(dm.TotalAmount * 2);
                            break;
                        case "Silver":
                            points = (int)(dm.TotalAmount * 2.5);
                            break;
                        case "Gold":
                            if (dm.VisitCount > 50)
                                points = (int)(dm.TotalAmount * 3.5);
                            else
                                points = (int)(dm.TotalAmount * 3);
                            break;
                        case "Platinum":
                            points = (int)(dm.TotalAmount * 4);
                            break;
                        case "Diamond":
                            if (dm.VisitCount > 100)
                                points = (int)(dm.TotalAmount * 6);
                            else
                                points = (int)(dm.TotalAmount * 5);
                            break;
                    }

                    break;
                case 3:
                    points = (int)(dm.TotalAmount * 1.5);
                    break;
                case 5:
                    points = (int)(dm.TotalAmount * 1.8);
                    break;
            }

            if (dm.IsBirthday) points += 500;

            if (dm.ReferralCount > 0) points += dm.ReferralCount * 50;

            return points;
        }
    }
}
