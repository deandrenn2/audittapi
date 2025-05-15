import { ReactNode } from "react";

export interface permissionsModel{
    [x: string]: ReactNode;
    checked: boolean | undefined;
    id?: number;
    name: string,
    description: string;
}