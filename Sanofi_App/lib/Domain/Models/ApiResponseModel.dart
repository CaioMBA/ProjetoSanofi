class ApiResponse{
  final int statusCode;
  final String Body;
  final String? message;

  ApiResponse({required this.statusCode, required this.Body, this.message});
}