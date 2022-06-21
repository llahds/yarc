import { KeyValueModel, PostedBy } from "./common";

export interface Post extends EditPost {
    id: number;
    forum: KeyValueModel;
    ups: number;
    downs: number;
    postedBy: PostedBy;
    createdOn: Date;
    canReport: boolean;
}

export interface EditPost {
    title: string;
    text: string;
}