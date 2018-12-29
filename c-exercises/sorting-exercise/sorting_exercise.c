#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct {
  int width;
  int height;
} rectangle;

typedef struct {
  int no;
  const char *name;
} member;

int compare_scores(const void* score_a, const void* score_b)
{
  int a = *(int*)score_a;
  int b = *(int*)score_b;
  return a - b;
}

int compare_areas(const void* a, const void* b)
{
  rectangle* ra = (rectangle*)a;
  rectangle* rb = (rectangle*)b;
  int area_a = ra->width * ra->height;
  int area_b = rb->width * rb->height;
  return area_a - area_b;
}

int compare_names(const void* a, const void* b)
{
  char** sa = (char**)a;
  char** sb = (char**)b;
  return strcmp(*sa, *sb);
}

int sort_members(const void* a, const void* b)
{
  member* member_a = (member*)a;
  member* member_b = (member*)b;
  return member_a->no - member_b->no;
}

int main()
{
  member members[] = {{3, "Mona"}, {2, "Bella"}, {1, "Mark"}, {4, "George"}};
  int i;
  qsort(members, 4, sizeof(member), sort_members);
  for (i = 0; i < 4; i++) {
    printf("%i. %s\n", members[i].no, members[i].name);
  }
  return 0;
}
