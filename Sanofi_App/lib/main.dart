import 'package:flutter/material.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';
import 'Pages/Login.dart';

void main() {
  runApp(const MyApp());
}


class MyApp extends StatelessWidget {
  const MyApp({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Sanofi Micro Manager',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        primarySwatch: GlobalSchematics().primaryColor,
      ),
      home: const LoginPage(),
    );
  }
}
