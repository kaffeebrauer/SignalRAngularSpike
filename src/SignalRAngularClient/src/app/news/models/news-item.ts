export class NewsItem {
    public headline = '';
    public newsText = '';
    public source = '';
    public equity = '';

    constructor() {
    }

    AddData(headline: string, newsText: string, source: string, equity: string) {
        this.headline = headline;
        this.newsText = newsText;
        this.source = source;
        this.equity = equity;
    }
}
