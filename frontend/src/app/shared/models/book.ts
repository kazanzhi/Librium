import { Category } from "./category";

export interface Book {
  id: string;
  title: string;
  author: string;
  categories: Category[];
  publishedYear: number;
}