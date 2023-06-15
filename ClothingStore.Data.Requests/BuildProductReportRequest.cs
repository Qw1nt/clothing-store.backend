namespace ClothingStore.Data.Requests;

public record BuildProductReportRequest(List<int> ProductsIds, DateTime StartDate, DateTime EndDate);
