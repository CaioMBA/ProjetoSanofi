import 'package:flutter/material.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';

class CommonBorderRoundedButton extends StatelessWidget {
  final Function()? onTap;
  final String? text;
  const CommonBorderRoundedButton({super.key, required this.onTap, this.text});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
        onTap: onTap,
        child: Container(
            padding: EdgeInsets.symmetric(vertical:  MediaQuery.of(context).size.height * 0.02,
                                          horizontal: MediaQuery.of(context).size.width * 0.02),
            margin: EdgeInsets.symmetric(horizontal: MediaQuery.of(context).size.width * 0.01),
            decoration: BoxDecoration(
              boxShadow: [
                BoxShadow(
                  color: GlobalSchematics().primaryColor.withOpacity(0.5),
                  spreadRadius: 3,
                  blurRadius: 5,
                )
              ],
                color: GlobalSchematics().primaryColor,
                borderRadius: BorderRadius.circular(20)),
            child: Center(
                child: FittedBox(
                    fit: BoxFit.fitWidth,
                    child: Text(
                      text ?? 'Próximo',
                      style: const TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 25),
                    )))));
  }
}
