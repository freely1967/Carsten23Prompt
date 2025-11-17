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
                case CustomerCategory.Regular:
                    points = (int)(dm.TotalAmount * 1m);
                    if (dm.HasApp)
                    {
                        if (dm.EmailSubscribed)
                        {
                            if (dm.VisitCount > 10)
                                points = (int)(dm.TotalAmount * 2m);
                            else
                                points = (int)(dm.TotalAmount * 1.5m);
                        }
                        else
                        {
                            points = (int)(dm.TotalAmount * 1.2m);
                        }
                    }

                    break;
                case CustomerCategory.VIP:
                    switch (dm.MembershipLevel)
                    {
                        case "Bronze":
                            points = (int)(dm.TotalAmount * 2m);
                            break;
                        case "Silver":
                            points = (int)(dm.TotalAmount * 2.5m);
                            break;
                        case "Gold":
                            if (dm.VisitCount > 50)
                                points = (int)(dm.TotalAmount * 3.5m);
                            else
                                points = (int)(dm.TotalAmount * 3m);
                            break;
                        case "Platinum":
                            points = (int)(dm.TotalAmount * 4m);
                            break;
                        case "Diamond":
                            if (dm.VisitCount > 100)
                                points = (int)(dm.TotalAmount * 6m);
                            else
                                points = (int)(dm.TotalAmount * 5m);
                            break;
                    }

                    break;
                case CustomerCategory.Employee:
                    points = (int)(dm.TotalAmount * 1.5m);
                    break;
                case CustomerCategory.Student:
                    points = (int)(dm.TotalAmount * 1.8m);
                    break;
            }

            if (dm.IsBirthday) points += 500;

            if (dm.ReferralCount > 0) points += dm.ReferralCount * 50;

            return points;
        }
    }
}
