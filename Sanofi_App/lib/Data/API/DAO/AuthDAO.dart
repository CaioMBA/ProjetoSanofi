import 'dart:convert';

import 'package:sanofi_app/Domain/ApiResponseModel.dart';

import '../../../Domain/AuthModels.dart';
import '../DefaultAPIAccess.dart';

class AuthDao{
  Future<AuthTokenResponse> login(String user, String password) async {
    final Map<String, String> headers = {
      'Authorization': 'Basic ${base64Encode(utf8.encode('$user:$password'))}'
    };

    final ApiResponse response = await apiRequest('POST', 'http://maincaitoserver.ddns.net:7000/api/Authorization/Auth', headers, null, null);
    final AuthTokenResponse authTokenResponse = AuthTokenResponse.fromJson(jsonDecode(response.responseBody));
    return authTokenResponse;
  }
}