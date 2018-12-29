#include <stdio.h>
#include <stdlib.h>
#include <time.h>

char* now()
{
  time_t t;
  time(&t);
  return asctime(localtime(&t));
}

int main()
{
  char comment[80];
  char cmd[120];
  fgets(comment, 80, stdin);
  // The book mispredicted the line break behaviour
  sprintf(cmd, "echo '%s %s' >> reports.log", comment, now());
  printf("%s", cmd);
  system("echo Hello again >> reports.log");
  return 0;
}
