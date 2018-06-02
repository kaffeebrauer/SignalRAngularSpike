import { NewsItem } from '../models/news-item';

export interface NewsState {
    news: NewsStateContainer
};

export interface NewsStateContainer {
    newsItems: NewsItem[],
    equities: string[]
};