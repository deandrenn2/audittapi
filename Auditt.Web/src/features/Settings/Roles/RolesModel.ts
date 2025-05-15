import { permissionsModel } from "./Permission/PermissionModel";

export interface RolesModel {
id?: number,
name: string,
description: string,
permissions: permissionsModel[];
}