import { EquivalenceModel } from "./Equivalence/EquivalenceModel";

export interface ScaleModel {
    id?: number;
    name: string;
    equivalences?: EquivalenceModel[];
}