#include <string.h>
#include "url_validator.h"

char string[255];
char *invalid_char;

void replace(int character, int new_character)
{
  while(strchr(string, character)) {
    invalid_char = strchr(string, character);
    *invalid_char = new_character;
  }
}

char *validate(char input_string[])
{
  strcpy(string, input_string);
  replace(' ', '_');
  return string;
}
