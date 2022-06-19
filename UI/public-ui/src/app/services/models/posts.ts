import { KeyValueModel, PostedBy } from "./common";

export interface Post {
    id: number;
    forum: KeyValueModel;
    ups: number;
    downs: number;
    postedBy: PostedBy;
    createdOn: Date;
    title: string;
    text: string;
}
