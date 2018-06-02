import { NewsState } from './news.state';
import * as newsAction from './news.action';

export const initialState: NewsState = {
    news: {
        newsItems: [],
        equities: ['WPL', 'TWE', 'MQG', 'RIO']
    }
};

export function newsReducer(state = initialState, action: newsAction.Actions): NewsState {
    switch (action.type) {

        case newsAction.RECEIVED_GROUP_JOINED:
            return Object.assign({}, state, {
                news: {
                    newsItems: state.news.newsItems,
                    equities: (state.news.equities.indexOf(action.equity) > -1) ? state.news.equities : state.news.equities.concat(action.equity)
                }
            });

        case newsAction.RECEIVED_NEWS_ITEM:
            return Object.assign({}, state, {
                news: {
                    newsItems: state.news.newsItems.concat(action.newsItem),
                    equities: state.news.equities
                }
            });

        case newsAction.RECEIVED_GROUP_HISTORY:
            return Object.assign({}, state, {
                news: {
                    newsItems: action.newsItems,
                    equities: state.news.equities
                }
            });

        case newsAction.RECEIVED_GROUP_LEFT:
            const data = [];
            for (const entry of state.news.equities) {
                if (entry !== action.equity) {
                    data.push(entry);
                }
            }
            console.log(data);
            return Object.assign({}, state, {
                news: {
                    newsItems: state.news.newsItems,
                    equity: data
                }
            });

        case newsAction.SELECTALL_GROUPS_COMPLETE:
            return Object.assign({}, state, {
                news: {
                    newsItems: state.news.newsItems,
                    equities: action.equities
                }
            });

        default:
            return state;

    }
}
